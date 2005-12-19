//
// ImageWriter.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Generated by /CodeGen/cecil-gen.rb do not edit
// <%=Time.now%>
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

	using Mono.Cecil.Metadata;

	class ImageWriter : BaseImageVisitor {

		Image m_img;
		AssemblyKind m_kind;
		MetadataWriter m_mdWriter;
		MemoryBinaryWriter m_binaryWriter;

		Section m_textSect;
		MemoryBinaryWriter m_textWriter;
		Section m_relocSect;
		MemoryBinaryWriter m_relocWriter;

		public ImageWriter (MetadataWriter writer, AssemblyKind kind, MemoryBinaryWriter bw)
		{
			m_mdWriter= writer;
			m_img = writer.GetMetadataRoot ().GetImage ();
			m_kind = kind;
			m_binaryWriter = bw;

			m_textWriter = new MemoryBinaryWriter ();
			m_textWriter.BaseStream.Position = 80;
			m_relocWriter = new MemoryBinaryWriter ();
		}

		public Image GetImage ()
		{
			return m_img;
		}

		public MemoryBinaryWriter GetTextWriter ()
		{
			return m_textWriter;
		}

		public uint GetAligned (uint integer, uint alignWith)
		{
			return (integer + alignWith - 1) & ~(alignWith - 1);
		}

		public void Initialize ()
		{
			Image img = m_img;
			uint sectAlign = img.PEOptionalHeader.NTSpecificFields.SectionAlignment;
			uint fileAlign = img.PEOptionalHeader.NTSpecificFields.FileAlignment;

			m_textSect = img.TextSection;
			foreach (Section s in img.Sections)
				if (s.Name == Section.Relocs)
					m_relocSect = s;

			// size computations, fields setting, etc.
			uint nbSects = (uint) img.Sections.Count;
			img.PEFileHeader.NumberOfSections = (ushort) nbSects;

			// build the reloc section data
			uint relocSize = 12;
			m_relocWriter.Write ((uint) 0);
			m_relocWriter.Write (relocSize);
			m_relocWriter.Write ((ushort) 0);
			m_relocWriter.Write ((ushort) 0);

			m_textSect.VirtualSize = (uint) m_textWriter.BaseStream.Length;
			m_relocSect.VirtualSize = (uint) m_relocWriter.BaseStream.Length;

			// start counting before sections headers
			// section start + section header sixe * number of sections
			uint headersEnd = 0x178 + 0x28 * nbSects;
			uint fileOffset = headersEnd;
			uint sectOffset = sectAlign;
			uint imageSize = 0;

			foreach (Section sect in img.Sections) {
				fileOffset = GetAligned (fileOffset, fileAlign);
				sectOffset = GetAligned (sectOffset, sectAlign);

				sect.PointerToRawData = new RVA (fileOffset);
				sect.VirtualAddress = new RVA (sectOffset);
				sect.SizeOfRawData = GetAligned (sect.VirtualSize, fileAlign);

				fileOffset += sect.SizeOfRawData;
				sectOffset += sect.SizeOfRawData;
				imageSize += GetAligned (sect.SizeOfRawData, sectAlign);
			}

			if (m_textSect.VirtualAddress.Value != 0x2000)
				throw new ImageFormatException ("Wrong RVA for .text section");

			img.PEOptionalHeader.StandardFields.CodeSize = GetAligned (
				m_textSect.SizeOfRawData, fileAlign);
			img.PEOptionalHeader.StandardFields.InitializedDataSize = 0x200; // + rsrc.SizeOfRawData ?
			img.PEOptionalHeader.StandardFields.BaseOfCode = m_textSect.VirtualAddress;
			img.PEOptionalHeader.StandardFields.BaseOfData = m_relocSect.VirtualAddress;

			imageSize += headersEnd;
			img.PEOptionalHeader.NTSpecificFields.ImageSize = GetAligned (imageSize, sectAlign);

			img.PEOptionalHeader.DataDirectories.BaseRelocationTable = new DataDirectory (
				m_relocSect.VirtualAddress, m_relocSect.VirtualSize);

			if (m_kind == AssemblyKind.Dll) {
				img.PEFileHeader.Characteristics = ImageCharacteristics.CILOnlyDll;
				img.HintNameTable.RuntimeMain = HintNameTable.RuntimeMainDll;
				img.PEOptionalHeader.NTSpecificFields.DLLFlags = 0x400;
			} else {
				img.PEFileHeader.Characteristics = ImageCharacteristics.CILOnlyExe;
				img.HintNameTable.RuntimeMain = HintNameTable.RuntimeMainExe;
			}

			switch (m_kind) {
			case AssemblyKind.Dll :
			case AssemblyKind.Console :
				img.PEOptionalHeader.NTSpecificFields.SubSystem = SubSystem.WindowsCui;
				break;
			case AssemblyKind.Windows :
				img.PEOptionalHeader.NTSpecificFields.SubSystem = SubSystem.WindowsGui;
				break;
			}

			RVA importTable = new RVA (img.TextSection.VirtualAddress + m_mdWriter.ImportTablePosition);

			img.PEOptionalHeader.DataDirectories.ImportTable = new DataDirectory (importTable, 0x57);

			img.ImportTable.ImportLookupTable = new RVA ((uint) importTable + 0x28);

			img.ImportLookupTable.HintNameRVA = img.ImportAddressTable.HintNameTableRVA =
				new RVA ((uint) img.ImportTable.ImportLookupTable + 0x14);
			img.ImportTable.Name = new RVA ((uint) img.ImportLookupTable.HintNameRVA + 0xe);
		}

		public override void VisitDOSHeader (DOSHeader header)
		{
			m_binaryWriter.Write (header.Start);
			m_binaryWriter.Write (header.Lfanew);
			m_binaryWriter.Write (header.End);

			m_binaryWriter.Write ((ushort) 0x4550);
			m_binaryWriter.Write ((ushort) 0);
		}
<% $headers.each { |name, header| if name != "Section" && name != "CLIHeader" && name != "DebugHeader" %>
		public override void Visit<%=name.index('.') ? name[(name.index('.') + 1)..name.length] : name%> (<%=name%> header)
		{<% header.fields.each { |field| %>
			<%=field.write_binary("header", "m_binaryWriter")%>;<% } %>
		}
<% end } ; cur_header = $headers["Section"] %>
		public override void VisitSection (Section sect)
		{
			foreach (char c in sect.Name)
				m_binaryWriter.Write (c);
			int more = 8 - sect.Name.Length;
			for (int i = 0; i < more; i++)
				m_binaryWriter.Write ((byte) 0);
<% cur_header.fields.each { |field| %>
			<%=field.write_binary("sect", "m_binaryWriter")%>;<% } %>
		}

		public override void VisitImportAddressTable (ImportAddressTable iat)
		{
			m_textWriter.BaseStream.Position = 0;
			m_textWriter.Write (iat.HintNameTableRVA.Value);
			m_textWriter.Write (new byte [4]);
		}
<% cur_header = $headers["CLIHeader"] %>
		public override void VisitCLIHeader (CLIHeader header)
		{<% cur_header.fields.each { |field| %>
			<%=field.write_binary("header", "m_textWriter")%>;<% } %>
		}
<% cur_header = $headers["DebugHeader"] %>
		public override void VisitDebugHeader (DebugHeader header)
		{
			m_textWriter.BaseStream.Position = m_mdWriter.DebugHeaderPosition;
			uint sizeUntilData = 0x1c;
			header.AddressOfRawData = m_img.TextSection.VirtualAddress + m_mdWriter.DebugHeaderPosition + sizeUntilData;
			header.PointerToRawData = 0x200 + m_mdWriter.DebugHeaderPosition + sizeUntilData;
			header.SizeOfData = 0x18 + (uint) header.FileName.Length + 1;

			m_textWriter.Write (header.Characteristics);
			m_textWriter.Write (header.TimeDateStamp);
			m_textWriter.Write (header.MajorVersion);
			m_textWriter.Write (header.MinorVersion);
			m_textWriter.Write ((uint) header.Type);
			m_textWriter.Write (header.SizeOfData);
			m_textWriter.Write (header.AddressOfRawData.Value);
			m_textWriter.Write (header.PointerToRawData);

			m_textWriter.Write (header.Magic);
			m_textWriter.Write (header.Signature.ToByteArray ());
			m_textWriter.Write (header.Age);
			foreach (char c in header.FileName)
				m_textWriter.Write (c);
			m_textWriter.Write ((byte) 0);
		}

		public override void VisitImportTable (ImportTable it)
		{
			m_textWriter.BaseStream.Position = m_mdWriter.ImportTablePosition;
			m_textWriter.Write (it.ImportLookupTable.Value);
			m_textWriter.Write (it.DateTimeStamp);
			m_textWriter.Write (it.ForwardChain);
			m_textWriter.Write (it.Name.Value);
			m_textWriter.Write (it.ImportAddressTable.Value);
			m_textWriter.Write (new byte [20]);
		}

		public override void VisitImportLookupTable (ImportLookupTable ilt)
		{
			m_textWriter.Write (ilt.HintNameRVA.Value);
			m_textWriter.Write (new byte [16]);
		}

		public override void VisitHintNameTable (HintNameTable hnt)
		{
			m_textWriter.Write (hnt.Hint);
			foreach (char c in hnt.RuntimeMain)
				m_textWriter.Write (c);
			m_textWriter.Write ('\0');
			foreach (char c in hnt.RuntimeLibrary)
				m_textWriter.Write (c);
			m_textWriter.Write ('\0');
			m_textWriter.Write (new byte [4]);

			// patch header with ep rva
			RVA ep = m_img.TextSection.VirtualAddress +
				(uint) m_textWriter.BaseStream.Position;
			long pos = m_binaryWriter.BaseStream.Position;
			m_binaryWriter.BaseStream.Position = 0xa8;
			m_binaryWriter.Write (ep.Value);
			m_binaryWriter.BaseStream.Position = pos;

			// patch reloc Sect with ep
			uint reloc = (ep.Value + 2) % 0x1000;
			uint rva = (ep.Value + 2) - reloc;

			m_relocWriter.BaseStream.Position = 0;
			m_relocWriter.Write (rva);
			m_relocWriter.BaseStream.Position = 8;
			m_relocWriter.Write ((ushort) ((3 << 12) | reloc));

			m_textWriter.Write (hnt.EntryPoint);
			m_textWriter.Write (hnt.RVA);
		}

		public override void TerminateImage (Image img)
		{
			m_binaryWriter.BaseStream.Position = 0x200;

			m_binaryWriter.Write (m_textWriter);
			m_binaryWriter.Write (new byte [
				m_textSect.SizeOfRawData - m_textWriter.BaseStream.Length]);

			m_binaryWriter.Write (m_relocWriter);
			m_binaryWriter.Write (new byte [
				m_relocSect.SizeOfRawData - m_relocWriter.BaseStream.Length]);
		}
	}
}
