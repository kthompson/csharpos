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
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * <%=Time.now%>
 *
 *****************************************************************************/

namespace Mono.Cecil.Binary {

    using System;
    using System.IO;

    using Mono.Cecil.Metadata;

    internal sealed class ImageReader : IBinaryVisitor {

        private BinaryReader m_binaryReader;
        private Image m_image;

        public ImageReader (string file)
        {
            m_image = Image.GetImage (file);
            m_image.Accept (this);
        }

        public Image GetImage ()
        {
            return m_image;
        }

        public BinaryReader GetReader ()
        {
            return m_binaryReader;
        }

        public void Visit (Image img)
        {
            m_binaryReader = new BinaryReader (new FileStream (
                img.FileInformation.FullName, FileMode.Open,
                FileAccess.Read, FileShare.Read));
        }

        public void Visit (DOSHeader header)
        {
            header.Start = m_binaryReader.ReadBytes (60);
            header.Lfanew = m_binaryReader.ReadUInt32 ();
            header.End = m_binaryReader.ReadBytes (64);

            m_binaryReader.BaseStream.Position = header.Lfanew;

            if ( m_binaryReader.ReadUInt16 () != 0x4550 ||
                m_binaryReader.ReadUInt16 () != 0)

                throw new ImageFormatException ("Invalid PE File Signature");
        }

        public void Visit (PEOptionalHeader header)
        {
        }
<% $headers.each { |name, header| if name != "Section" && name != "CLIHeader" %>
        public void Visit (<%=name%> header)
        {<% header.fields.each { |field| %>
            header.<%=field.property_name%> = <%=field.read_binary("m_binaryReader")%>;<% } %>
        }
<% end } %>
        public void Visit (SectionCollection coll)
        {
            for (int i = 0; i < m_image.PEFileHeader.NumberOfSections; i++)
                coll.Add (new Section ());
        }
<% cur_header = $headers["Section"] %>
        public void Visit (Section sect)
        {
            char [] name, buffer = new char [8];
            int read = 0;
            while (read < 8) {
                char cur = (char) m_binaryReader.ReadSByte ();
                if (cur == '\0')
                    break;
                buffer [read++] = cur;
            }
            name = new char [read];
            Array.Copy (buffer, name, read);
            sect.Name = read == 0 ? string.Empty : new string (name);
            m_binaryReader.BaseStream.Position += 8 - read - 1;
<% cur_header.fields.each { |field| %>            sect.<%=field.property_name%> = <%=field.read_binary("m_binaryReader")%>;<% print("\n") } %>
            m_image.Sections [sect.Name] = sect;
        }
<% cur_header = $headers["CLIHeader"] %>
        public void Visit (CLIHeader header)
        {
            if (m_image.PEOptionalHeader.DataDirectories.CLIHeader == DataDirectory.Zero)
                throw new ImageFormatException ("Non Pure CLI Image");

            m_binaryReader.BaseStream.Position = m_image.ResolveVirtualAddress (
                m_image.PEOptionalHeader.DataDirectories.CLIHeader.VirtualAddress);
<% cur_header.fields.each { |field| %>            header.<%=field.property_name%> = <%=field.read_binary("m_binaryReader")%>;<% print("\n") } %>
            if (header.StrongNameSignature != DataDirectory.Zero) {
                m_binaryReader.BaseStream.Position = m_image.ResolveVirtualAddress (
                    header.StrongNameSignature.VirtualAddress);
                header.ImageHash = m_binaryReader.ReadBytes ((int) header.StrongNameSignature.Size);
            } else {
                header.ImageHash = new byte [0];
            }
            m_binaryReader.BaseStream.Position = m_image.ResolveVirtualAddress (
                m_image.CLIHeader.Metadata.VirtualAddress);
            MetadataReader mrv = new MetadataReader (this);
            m_image.MetadataRoot.Accept (mrv);
        }

        public void Terminate (Image img)
        {
            m_binaryReader.Close();
        }
    }
}
