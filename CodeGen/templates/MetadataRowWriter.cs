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

	internal sealed class MetadataRowWriter : IMetadataRowVisitor {

		private MetadataTableWriter m_mtwv;
		private MetadataRoot m_metadataRoot;

		public MetadataRowWriter (MetadataTableWriter mtwv)
		{
			m_mtwv = mtwv;
			m_metadataRoot = mtwv.GetMetadataRoot ();
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

<% } %>		public void Visit (RowCollection coll)
		{
		}

<% $tables.each { |table| %>		public void Visit (<%=table.row_name%> row)
		{
		}

<% } %>		public void Terminate (RowCollection coll)
		{
		}
	}
}