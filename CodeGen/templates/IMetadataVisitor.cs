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

    internal interface IMetadataVisitor {
        void Visit(MetadataRoot root);
        void Visit(MetadataRoot.MetadataRootHeader header);
        void Visit(MetadataStreamCollection streams);
        void Visit(MetadataStream stream);
        void Visit(MetadataStream.MetadataStreamHeader header);
        void Visit(GuidHeap heap);
        void Visit(StringsHeap heap);
        void Visit(TablesHeap heap);
        void Visit(BlobHeap heap);
        void Visit(UserStringsHeap heap);

        void Terminate(MetadataStreamCollection streams);
        void Terminate(MetadataRoot root);
    }

    internal interface IMetadataTableVisitor {
        void Visit(TableCollection coll);

<% $tables.each { |table| %>        void Visit(<%= table.table_name %> table);
<% } %>     void Terminate(TableCollection coll);
        IMetadataRowVisitor GetRowVisitor();
}

    internal interface IMetadataRowVisitor {
        void Visit(RowCollection coll);

<% $tables.each { |table| %>        void Visit(<%= table.row_name %> row);
<% } %>     void Terminate(RowCollection coll);
    }
}
