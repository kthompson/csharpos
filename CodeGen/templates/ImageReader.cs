//
// ImageReader.cs
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

	using System;
	using System.IO;
	using System.Text;

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
<% $headers.each { |name, header| if name != "Section" && name != "CLIHeader" && name != "DebugHeader" %>
		public override void Visit<%=name.index('.') ? name[(name.index('.') + 1)..name.length] : name%> (<%=name%> header)
		{<% header.fields.each { |field| %>
			header.<%=field.property_name%> = <%=field.read_binary("m_binaryReader")%>;<% } %>
		}
<% end } %>
		public override void VisitSectionCollection (SectionCollection coll)
		{
			for (int i = 0; i < m_image.PEFileHeader.NumberOfSections; i++)
				coll.Add (new Section ());
		}
<% cur_header = $headers["Section"] %>
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
<% cur_header.fields.each { |field| %>			sect.<%=field.property_name%> = <%=field.read_binary("m_binaryReader")%>;
<% } %>		}

		public override void VisitImportAddressTable (ImportAddressTable iat)
		{
			m_binaryReader.BaseStream.Position = m_image.ResolveTextVirtualAddress (
				m_image.PEOptionalHeader.DataDirectories.IAT.VirtualAddress);

			iat.HintNameTableRVA = new RVA (m_binaryReader.ReadUInt32 ());
		}
<% cur_header = $headers["CLIHeader"] %>
		public override void VisitCLIHeader (CLIHeader header)
		{
			if (m_image.PEOptionalHeader.DataDirectories.CLIHeader == DataDirectory.Zero)
				throw new ImageFormatException ("Non Pure CLI Image");

			if (m_image.PEOptionalHeader.DataDirectories.Debug != DataDirectory.Zero) {
				m_image.DebugHeader = new DebugHeader ();
				VisitDebugHeader (m_image.DebugHeader);
			}

			m_binaryReader.BaseStream.Position = m_image.ResolveTextVirtualAddress (
				m_image.PEOptionalHeader.DataDirectories.CLIHeader.VirtualAddress);
<% cur_header.fields.each { |field| %>			header.<%=field.property_name%> = <%=field.read_binary("m_binaryReader")%>;
<% } %>
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
<% cur_header = $headers["DebugHeader"] %>
		public override void VisitDebugHeader (DebugHeader header)
		{
			if (m_image.PEOptionalHeader.DataDirectories.Debug == DataDirectory.Zero)
				return;

			long pos = m_binaryReader.BaseStream.Position;

			m_binaryReader.BaseStream.Position = m_image.ResolveVirtualAddress (
				m_image.PEOptionalHeader.DataDirectories.Debug.VirtualAddress);
<% cur_header.fields.each { |field| %>			header.<%=field.property_name%> = <%=field.read_binary("m_binaryReader")%>;
<% } %>
			m_binaryReader.BaseStream.Position = m_image.ResolveVirtualAddress (
				m_image.DebugHeader.AddressOfRawData);

			header.Magic = m_binaryReader.ReadUInt32 ();
			header.Signature = new Guid (m_binaryReader.ReadBytes (16));
			header.Age = m_binaryReader.ReadUInt32 ();

			StringBuilder buffer = new StringBuilder ();
			while (true) {
				byte cur =  m_binaryReader.ReadByte ();
				if (cur == 0)
					break;
				buffer.Append ((char) cur);
			}
			header.FileName = buffer.ToString ();

			m_binaryReader.BaseStream.Position = pos;
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
