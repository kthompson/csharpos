/*
 * Copyright (c) 2004, 2005 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jbevain@gmail.com)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * <%=Time.now%>
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

	using System;
	using System.Collections;

	using Mono.Cecil.Binary;

	internal sealed class MetadataRowWriter : BaseMetadataRowVisitor {

		private MetadataRoot m_root;
		private MemoryBinaryWriter m_binaryWriter;
		private IDictionary m_ciCache;

		private int m_blobHeapIdxSz;
		private int m_stringsHeapIdxSz;
		private int m_guidHeapIdxSz;

		public MetadataRowWriter (MetadataTableWriter mtwv)
		{
			m_binaryWriter = mtwv.GetWriter ();
			m_root = mtwv.GetMetadataRoot ();
			m_ciCache = new Hashtable ();
		}

		private void WriteBlobPointer (uint pointer)
		{
			WriteByIndexSize (pointer, m_blobHeapIdxSz);
		}

		private void WriteStringPointer (uint pointer)
		{
			WriteByIndexSize (pointer, m_stringsHeapIdxSz);
		}

		private void WriteGuidPointer (uint pointer)
		{
			WriteByIndexSize (pointer, m_guidHeapIdxSz);
		}

		private void WriteTablePointer (uint pointer, Type table)
		{
			WriteByIndexSize (pointer, GetNumberOfRows (table) < (1 << 16) ? 2 : 4);
		}

		private void WriteMetadataToken (MetadataToken token, CodedIndex ci)
		{
			WriteByIndexSize (Utilities.CompressMetadataToken (ci, token),
				Utilities.GetCodedIndexSize (
					ci, new Utilities.TableRowCounter (GetNumberOfRows), m_ciCache));
		}

		private int GetNumberOfRows (Type table)
		{
			IMetadataTable t = m_root.Streams.TablesHeap [table];
			if (t == null || t.Rows == null)
				return 0;
			return t.Rows.Count;
		}

		private void WriteByIndexSize (uint value, int size)
		{
			if (size == 4)
				m_binaryWriter.Write (value);
			else if (size == 2)
				m_binaryWriter.Write ((ushort) value);
			else
				throw new MetadataFormatException ("Non valid size for indexing");
		}

<% $tables.each { |table|
		parameters = ""
		table.columns.each { |col|
			parameters += col.type
			parameters += " "
			parameters += col.field_name[1..col.field_name.length]
			parameters += ", " if (table.columns.last != col)
		}
%>		public <%=table.row_name%> Create<%=table.row_name%> (<%=parameters%>)
		{
			<%=table.row_name%> row = new <%=table.row_name%> ();
<% table.columns.each { |col| %>			row.<%=col.property_name%> = <%=col.field_name[1..col.field_name.length]%>;
<% } %>			return row;
		}

<% } %>		public override void Visit (RowCollection coll)
		{
			m_blobHeapIdxSz = m_root.Streams.BlobHeap != null ?
				m_root.Streams.BlobHeap.IndexSize : 2;
			m_stringsHeapIdxSz = m_root.Streams.StringsHeap != null ?
				m_root.Streams.StringsHeap.IndexSize : 2;
			m_guidHeapIdxSz = m_root.Streams.GuidHeap != null ?
				m_root.Streams.GuidHeap.IndexSize : 2;
		}

<% $tables.each { |table| %>		public override void Visit (<%=table.row_name%> row)
		{
<% table.columns.each { |col|
 if (col.target.nil?)
%>			<%=col.write_binary("row", "m_binaryWriter")%>;
<% elsif (col.target == "BlobHeap")
%>			WriteBlobPointer (row.<%=col.property_name%>);
<% elsif (col.target == "StringsHeap")
%>			WriteStringPointer (row.<%=col.property_name%>);
<% elsif (col.target == "GuidHeap")
%>			WriteGuidPointer (row.<%=col.property_name%>);
<% elsif (col.type == "MetadataToken")
%>			WriteMetadataToken (row.<%=col.property_name%>, CodedIndex.<%=col.target%>);
<% else
%>			WriteTablePointer (row.<%=col.property_name%>, typeof (<%=col.target%>Table));
<% end
}%>		}

<% } %>	}
}
