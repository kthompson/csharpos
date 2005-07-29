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
	using System.IO;

	internal sealed class MetadataTableWriter : IMetadataTableVisitor {

		private MetadataRoot m_metadataRoot;
		private TablesHeap m_heap;
		private MetadataRowWriter m_mrrw;
		private BinaryWriter m_binaryWriter;

		public MetadataTableWriter (MetadataWriter mrv)
		{
			m_metadataRoot = mrv.GetMetadataRoot ();
			m_heap = m_metadataRoot.Streams.TablesHeap;
			m_mrrw = new MetadataRowWriter (this);
			m_binaryWriter = mrv.GetWriter ();
		}

		public MetadataRoot GetMetadataRoot ()
		{
			return m_metadataRoot;
		}

		public IMetadataRowVisitor GetRowVisitor ()
		{
			return m_mrrw;
		}

		public BinaryWriter GetWriter ()
		{
			return m_binaryWriter;
		}

<% $tables.each { |table| %>		public <%=table.table_name%> Get<%=table.table_name%> ()
		{
			Type tt = typeof (<%=table.table_name%>);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as <%=table.table_name%>;

			<%=table.table_name%> table = new <%=table.table_name%> ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

<% } %>
		public void Visit (TableCollection coll)
		{
		}

<% $tables.each { |table| %>		public void Visit (<%=table.table_name%> table)
		{
		}

<% } %>		public void Terminate(TableCollection coll)
		{
		}
	}
}
