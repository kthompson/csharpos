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
 * Tue Mar 01 00:32:44 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

    [RId (0x10)]
    public sealed class FieldLayoutTable : IMetadataTable {

        private RowCollection m_rows;

        public FieldLayoutRow this [int index] {
            get { return m_rows [index] as FieldLayoutRow; }
            set { m_rows [index] = value; }
        }

        public RowCollection Rows {
            get { return m_rows; }
            set { m_rows = value; }
        }

        internal FieldLayoutTable ()
        {
        }

        public void Accept (IMetadataTableVisitor visitor)
        {
            visitor.Visit (this);
            this.Rows.Accept (visitor.GetRowVisitor ());
        }
    }

    public sealed class FieldLayoutRow : IMetadataRow {

        public static readonly int RowSize = 8;
        public static readonly int RowColumns = 2;

        public uint Offset;
        public uint Field;

        public int Size {
            get { return FieldLayoutRow.RowSize; }
        }

        public int Columns {
            get { return FieldLayoutRow.RowColumns; }
        }

        internal FieldLayoutRow ()
        {
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
