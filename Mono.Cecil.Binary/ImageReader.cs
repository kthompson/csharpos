//
// ImageReader.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Generated by /CodeGen/cecil-gen.rb do not edit
// Thu Sep 29 22:51:39 CEST 2005
//
// (C) 2005 Jb Evain
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

namespace Mono.Cecil.Binary {

	using System;
	using System.IO;
	using System.Text;

	using Mono.Cecil;
	using Mono.Cecil.Metadata;

	class ImageReader : BaseImageVisitor {

		MetadataReader m_mdReader;
		BinaryReader m_binaryReader;
		Image m_image;

		public MetadataReader MetadataReader {
			get { return m_mdReader; }
		}

		public Image Image {
			get { return m_image; }
		}

		public ImageReader (string file)
		{
			m_image = Image.GetImage (file);
			m_image.Accept (this);
		}

		public BinaryReader GetReader ()
		{
			return m_binaryReader;
		}

		public override void VisitImage (Image img)
		{
			m_binaryReader = new BinaryReader (
				new FileStream (img.FileInformation.FullName, FileMode.Open,
					FileAccess.Read, FileShare.Read));
			m_mdReader = new MetadataReader (this);
		}

		public override void VisitDOSHeader (DOSHeader header)
		{
			header.Start = m_binaryReader.ReadBytes (60);
			header.Lfanew = m_binaryReader.ReadUInt32 ();
			header.End = m_binaryReader.ReadBytes (64);

			m_binaryReader.BaseStream.Position = header.Lfanew;

			if (m_binaryReader.ReadUInt16 () != 0x4550 ||
				m_binaryReader.ReadUInt16 () != 0)

				throw new ImageFormatException ("Invalid PE File Signature");
		}

		public override void VisitPEFileHeader (PEFileHeader header)
		{
			header.Machine = m_binaryReader.ReadUInt16 ();
			header.NumberOfSections = m_binaryReader.ReadUInt16 ();
			header.TimeDateStamp = m_binaryReader.ReadUInt32 ();
			header.PointerToSymbolTable = m_binaryReader.ReadUInt32 ();
			header.NumberOfSymbols = m_binaryReader.ReadUInt32 ();
			header.OptionalHeaderSize = m_binaryReader.ReadUInt16 ();
			header.Characteristics = (Mono.Cecil.Binary.ImageCharacteristics) m_binaryReader.ReadUInt16 ();
		}

		public override void VisitNTSpecificFieldsHeader (PEOptionalHeader.NTSpecificFieldsHeader header)
		{
			header.ImageBase = m_binaryReader.ReadUInt32 ();
			header.SectionAlignment = m_binaryReader.ReadUInt32 ();
			header.FileAlignment = m_binaryReader.ReadUInt32 ();
			header.OSMajor = m_binaryReader.ReadUInt16 ();
			header.OSMinor = m_binaryReader.ReadUInt16 ();
			header.UserMajor = m_binaryReader.ReadUInt16 ();
			header.UserMinor = m_binaryReader.ReadUInt16 ();
			header.SubSysMajor = m_binaryReader.ReadUInt16 ();
			header.SubSysMinor = m_binaryReader.ReadUInt16 ();
			header.Reserved = m_binaryReader.ReadUInt32 ();
			header.ImageSize = m_binaryReader.ReadUInt32 ();
			header.HeaderSize = m_binaryReader.ReadUInt32 ();
			header.FileChecksum = m_binaryReader.ReadUInt32 ();
			header.SubSystem = (Mono.Cecil.Binary.SubSystem) m_binaryReader.ReadUInt16 ();
			header.DLLFlags = m_binaryReader.ReadUInt16 ();
			header.StackReserveSize = m_binaryReader.ReadUInt32 ();
			header.StackCommitSize = m_binaryReader.ReadUInt32 ();
			header.HeapReserveSize = m_binaryReader.ReadUInt32 ();
			header.HeapCommitSize = m_binaryReader.ReadUInt32 ();
			header.LoaderFlags = m_binaryReader.ReadUInt32 ();
			header.NumberOfDataDir = m_binaryReader.ReadUInt32 ();
		}

		public override void VisitStandardFieldsHeader (PEOptionalHeader.StandardFieldsHeader header)
		{
			header.Magic = m_binaryReader.ReadUInt16 ();
			header.LMajor = m_binaryReader.ReadByte ();
			header.LMinor = m_binaryReader.ReadByte ();
			header.CodeSize = m_binaryReader.ReadUInt32 ();
			header.InitializedDataSize = m_binaryReader.ReadUInt32 ();
			header.UninitializedDataSize = m_binaryReader.ReadUInt32 ();
			header.EntryPointRVA = new RVA (m_binaryReader.ReadUInt32 ());
			header.BaseOfCode = new RVA (m_binaryReader.ReadUInt32 ());
			header.BaseOfData = new RVA (m_binaryReader.ReadUInt32 ());
		}

		public override void VisitDataDirectoriesHeader (PEOptionalHeader.DataDirectoriesHeader header)
		{
			header.ExportTable = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.ImportTable = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.ResourceTable = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.ExceptionTable = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.CertificateTable = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.BaseRelocationTable = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.Debug = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.Copyright = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.GlobalPtr = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.TLSTable = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.LoadConfigTable = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.BoundImport = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.IAT = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.DelayImportDescriptor = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.CLIHeader = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.Reserved = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
		}

		public override void VisitSectionCollection (SectionCollection coll)
		{
			for (int i = 0; i < m_image.PEFileHeader.NumberOfSections; i++)
				coll.Add (new Section ());
		}

		public override void VisitSection (Section sect)
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
			Array.Copy (buffer, 0, name, 0, read);
			sect.Name = read == 0 ? string.Empty : new string (name);
			if (sect.Name == Section.Text)
				m_image.TextSection = sect;
			m_binaryReader.BaseStream.Position += 8 - read - 1;
			sect.VirtualSize = m_binaryReader.ReadUInt32 ();
			sect.VirtualAddress = new RVA (m_binaryReader.ReadUInt32 ());
			sect.SizeOfRawData = m_binaryReader.ReadUInt32 ();
			sect.PointerToRawData = new RVA (m_binaryReader.ReadUInt32 ());
			sect.PointerToRelocations = new RVA (m_binaryReader.ReadUInt32 ());
			sect.PointerToLineNumbers = new RVA (m_binaryReader.ReadUInt32 ());
			sect.NumberOfRelocations = m_binaryReader.ReadUInt16 ();
			sect.NumberOfLineNumbers = m_binaryReader.ReadUInt16 ();
			sect.Characteristics = (Mono.Cecil.Binary.SectionCharacteristics) m_binaryReader.ReadUInt32 ();

		}

		public override void VisitImportAddressTable (ImportAddressTable iat)
		{
			m_binaryReader.BaseStream.Position = m_image.ResolveTextVirtualAddress (
				m_image.PEOptionalHeader.DataDirectories.IAT.VirtualAddress);

			iat.HintNameTableRVA = new RVA (m_binaryReader.ReadUInt32 ());
		}

		public override void VisitCLIHeader (CLIHeader header)
		{
			if (m_image.PEOptionalHeader.DataDirectories.CLIHeader == DataDirectory.Zero)
				throw new ImageFormatException ("Non Pure CLI Image");

			m_binaryReader.BaseStream.Position = m_image.ResolveTextVirtualAddress (
				m_image.PEOptionalHeader.DataDirectories.CLIHeader.VirtualAddress);
			header.Cb = m_binaryReader.ReadUInt32 ();
			header.MajorRuntimeVersion = m_binaryReader.ReadUInt16 ();
			header.MinorRuntimeVersion = m_binaryReader.ReadUInt16 ();
			header.Metadata = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.Flags = (Mono.Cecil.Binary.RuntimeImage) m_binaryReader.ReadUInt32 ();
			header.EntryPointToken = m_binaryReader.ReadUInt32 ();
			header.Resources = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.StrongNameSignature = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.CodeManagerTable = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.VTableFixups = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.ExportAddressTableJumps = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());
			header.ManagedNativeHeader = new DataDirectory (
				new RVA (m_binaryReader.ReadUInt32 ()),
				m_binaryReader.ReadUInt32 ());

			if (header.StrongNameSignature != DataDirectory.Zero) {
				m_binaryReader.BaseStream.Position = m_image.ResolveTextVirtualAddress (
					header.StrongNameSignature.VirtualAddress);
				header.ImageHash = m_binaryReader.ReadBytes ((int) header.StrongNameSignature.Size);
			} else {
				header.ImageHash = new byte [0];
			}
			m_binaryReader.BaseStream.Position = m_image.ResolveTextVirtualAddress (
				m_image.CLIHeader.Metadata.VirtualAddress);
			m_image.MetadataRoot.Accept (m_mdReader);
		}

		public override void VisitImportTable (ImportTable it)
		{
			m_binaryReader.BaseStream.Position = m_image.ResolveTextVirtualAddress (
				m_image.PEOptionalHeader.DataDirectories.ImportTable.VirtualAddress);

			it.ImportLookupTable = new RVA (m_binaryReader.ReadUInt32 ());
			it.DateTimeStamp = m_binaryReader.ReadUInt32 ();
			it.ForwardChain = m_binaryReader.ReadUInt32 ();
			it.Name = new RVA (m_binaryReader.ReadUInt32 ());
			it.ImportAddressTable = new RVA (m_binaryReader.ReadUInt32 ());
		}

		public override void VisitImportLookupTable (ImportLookupTable ilt)
		{
			m_binaryReader.BaseStream.Position = m_image.ResolveTextVirtualAddress (
				m_image.ImportTable.ImportLookupTable.Value);

			ilt.HintNameRVA = new RVA (m_binaryReader.ReadUInt32 ());
		}

		public override void VisitHintNameTable (HintNameTable hnt)
		{
			m_binaryReader.BaseStream.Position = m_image.ResolveTextVirtualAddress (
				m_image.ImportAddressTable.HintNameTableRVA);

			hnt.Hint = m_binaryReader.ReadUInt16 ();
			
			byte [] bytes = m_binaryReader.ReadBytes (11);
			hnt.RuntimeMain = Encoding.ASCII.GetString (bytes, 0, bytes.Length);

			m_binaryReader.BaseStream.Position = m_image.ResolveTextVirtualAddress (
				m_image.ImportTable.Name);
				
			bytes = m_binaryReader.ReadBytes (11);
			hnt.RuntimeLibrary = Encoding.ASCII.GetString (bytes, 0, bytes.Length);

			m_binaryReader.BaseStream.Position = m_image.ResolveTextVirtualAddress (
				m_image.PEOptionalHeader.StandardFields.EntryPointRVA);
			hnt.EntryPoint = m_binaryReader.ReadUInt16 ();
			hnt.RVA = new RVA (m_binaryReader.ReadUInt32 ());
		}

		public override void TerminateImage (Image img)
		{
			m_binaryReader.Close ();
		}
	}
}
