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

namespace Mono.Cecil.Metadata {

    using System;
    using System.Collections;
    using System.IO;

    internal sealed class MetadataTableReader : IMetadataTableVisitor {

        private MetadataRoot m_metadataRoot;
        private TablesHeap m_heap;
        private MetadataRowReader m_mrrv;
        private BinaryReader m_binaryReader;

        public readonly IDictionary Rows = new Hashtable(<%=$tables.length%>);

        public MetadataTableReader(MetadataReader mrv) {
            m_metadataRoot = mrv.GetMetadataRoot();
            m_heap = m_metadataRoot.Streams.TablesHeap;
            m_binaryReader = new BinaryReader(new MemoryStream(m_heap.Data));
            m_binaryReader.BaseStream.Position = 24;
            m_mrrv = new MetadataRowReader(this);
        }

        public MetadataRoot GetMetadataRoot() {
            return m_metadataRoot;
        }

        public BinaryReader GetReader() {
            return m_binaryReader;
        }

        public IMetadataRowVisitor GetRowVisitor() {
            return m_mrrv;
        }

        private void ReadNumberOfRows(Type table) {
            this.Rows[table] = m_binaryReader.ReadInt32();
        }

        public int GetNumberOfRows(Type table) {
            object n = this.Rows[table];
            if (n != null) {
                return (int)n;
            }
            return 0;
        }
<%
    stables = $tables.sort { |a, b|
        eval(a.rid) <=> eval(b.rid)
    }
%>
        public void Visit(TableCollection coll) {
<% stables.each { |table| %>            if (m_heap.HasTable(typeof(<%=table.table_name%>))) {
                coll.Add(new <%=table.table_name%>());
                ReadNumberOfRows(typeof(<%=table.table_name%>));
            }
<% } %>        }

<% $tables.each { |table| %>        public void Visit(<%=table.table_name%> table) {
            table.Rows = new RowCollection(table);
            int number = GetNumberOfRows(typeof(<%=table.table_name%>));
            for (int i = 0 ; i < number ; i++) {
                table.Rows.Add(new <%=table.row_name%>());
            }
        }
<% print("\n") ; } %>        public void Terminate(TableCollection coll) {
            m_binaryReader.Close();
        }
    }
}
