/*
 * Copyright (c) 2004 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jb.evain@dotnetguru.org)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

    using System;
    using System.IO;
    using System.Text;

    using Mono.Cecil;
    using Mono.Cecil.Binary;

    internal sealed class MetadataReader : IMetadataVisitor {

        private BinaryReader m_binaryReader;
        private MetadataRoot m_root;

        public MetadataReader (ImageReader brv)
        {
            m_binaryReader = brv.GetReader ();
        }

        public MetadataRoot GetMetadataRoot ()
        {
            return m_root;
        }

        public void Visit (MetadataRoot root)
        {
            m_root = root;
            root.Header = new MetadataRoot.MetadataRootHeader ();
            root.Streams = new MetadataStreamCollection (root);
        }

        public void Visit (MetadataRoot.MetadataRootHeader header)
        {

            long headpos = m_binaryReader.BaseStream.Position;

            header.Signature = m_binaryReader.ReadUInt32 ();

            if (header.Signature != MetadataRoot.MetadataRootHeader.StandardSignature)
                throw new MetadataFormatException ("Wrong magic number");

            header.MajorVersion = m_binaryReader.ReadUInt16 ();
            header.MinorVersion = m_binaryReader.ReadUInt16 ();
            header.Reserved = m_binaryReader.ReadUInt32 ();

            // read version
            uint length = m_binaryReader.ReadUInt32 ();
            if (length != 0) {
                long pos = m_binaryReader.BaseStream.Position;

                byte [] version, buffer = new byte [length];
                int read = 0;
                while (read < length) {
                    byte cur = (byte)m_binaryReader.ReadSByte ();
                    if (cur == 0)
                        break;
                    buffer [read++] = cur;
                }
                version = new byte [read];
                Buffer.BlockCopy (buffer, 0, version, 0, read);
                header.Version = Encoding.UTF8.GetString (version);

                pos += length - headpos + 3;
                pos &= ~3;
                pos += headpos;

                m_binaryReader.BaseStream.Position = pos;
            } else {
                header.Version = string.Empty;
            }

            header.Flags = m_binaryReader.ReadUInt16 ();
            header.Streams = m_binaryReader.ReadUInt16 ();
        }

        public void Visit (MetadataStreamCollection coll)
        {
            for (int i = 0; i < m_root.Header.Streams; i++)
                coll.Add (new MetadataStream (m_root));
        }

        public void Visit (MetadataStream stream)
        {
            stream.Header = new MetadataStream.MetadataStreamHeader (stream);
        }

        public void Visit (MetadataStream.MetadataStreamHeader header)
        {
            header.Offset = m_binaryReader.ReadUInt32 ();
            header.Size = m_binaryReader.ReadUInt32 ();

            StringBuilder buffer = new StringBuilder ();
            while (true) {
                char cur = (char)m_binaryReader.ReadSByte ();
                if (cur == '\0')
                    break;
                buffer.Append (cur);
            }
            header.Name = buffer.ToString ();
            if (header.Name.Length == 0)
                throw new MetadataFormatException ("Invalid stream name");

            long rootpos = m_root.GetImage ().ResolveVirtualAddress (
                m_root.GetImage ().CLIHeader.Metadata.VirtualAddress);

            long curpos = m_binaryReader.BaseStream.Position;

            if (header.Size != 0)
                curpos -= rootpos;

            curpos += 3;
            curpos &= ~3;

            if (header.Size != 0)
                curpos += rootpos;

            m_binaryReader.BaseStream.Position = curpos;

            header.Stream.Heap = MetadataHeap.HeapFactory (header.Stream);
        }

        public void Visit (GuidHeap heap)
        {
            this.VisitHeap (heap);
        }

        public void Visit (StringsHeap heap)
        {
            this.VisitHeap (heap);

            if (heap.Data.Length < 1 && heap.Data [0] != 0)
                throw new MetadataFormatException ("Malformed #Strings heap");

            heap [(uint) 0] = string.Empty;

            using (BinaryReader br = new BinaryReader (new MemoryStream (heap.Data),
                                                       Encoding.UTF8)) {

                br.BaseStream.Position += 1;

                StringBuilder buffer = new StringBuilder ();
                for (int i = 1, index = 1 ; i < heap.Data.Length ; i++) {
                    char cur = br.ReadChar ();
                    if (cur == '\0' && buffer.Length > 0) {
                        heap [(uint) index] = buffer.ToString ();
                        buffer = new StringBuilder ();
                        index = i + 1;
                    } else {
                        buffer.Append (cur);
                    }
                }
            }
        }

        public void Visit (TablesHeap heap)
        {
            this.VisitHeap (heap);
            heap.Tables = new TableCollection (heap);

            using (BinaryReader br = new BinaryReader (
                       new MemoryStream (heap.Data))) {

                heap.Reserved = br.ReadUInt32 ();
                heap.MajorVersion = br.ReadByte ();
                heap.MinorVersion = br.ReadByte ();
                heap.HeapSizes = br.ReadByte ();
                heap.Reserved2 = br.ReadByte ();
                heap.Valid = br.ReadInt64 ();
                heap.Sorted = br.ReadInt64 ();
            }
        }

        public void Visit (BlobHeap heap)
        {
            this.VisitHeap (heap);
        }

        public void Visit (UserStringsHeap heap)
        {
            this.VisitHeap (heap);
        }

        private void VisitHeap (MetadataHeap heap)
        {
            long cursor = m_binaryReader.BaseStream.Position;

            m_binaryReader.BaseStream.Position = m_root.GetImage ().ResolveVirtualAddress (
                m_root.GetImage ().CLIHeader.Metadata.VirtualAddress)
                + heap.GetStream ().Header.Offset;

            heap.Data = m_binaryReader.ReadBytes ((int) heap.GetStream ().Header.Size);

            m_binaryReader.BaseStream.Position = cursor;
        }

        public void Terminate (MetadataStreamCollection coll)
        {
            StringsHeap sh = m_root.Streams.StringsHeap;
            GuidHeap gh = m_root.Streams.GuidHeap;
            BlobHeap bh = m_root.Streams.BlobHeap;
            TablesHeap th = m_root.Streams.TablesHeap;

            sh.IndexSize = ((th.HeapSizes & 0x01) > 0) ? 4 : 2;
            gh.IndexSize = ((th.HeapSizes & 0x02) > 0) ? 4 : 2;
            bh.IndexSize = ((th.HeapSizes & 0x04) > 0) ? 4 : 2;
        }

        public void Terminate (MetadataRoot root)
        {
            MetadataTableReader mtrv = new MetadataTableReader (this);
            root.Streams.TablesHeap.Tables.Accept (mtrv);
        }
    }
}
