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
 * Thu Feb 24 01:20:27 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

    [RId (0x0f)]
    internal sealed class ClassLayoutTable : IMetadataTable {

        private RowCollection m_rows;

        public ClassLayoutRow this [int index] {
            get { return m_rows [index] as ClassLayoutRow; }
            set { m_rows [index] = value; }
        }

        public RowCollection Rows {
            get { return m_rows; }
            set { m_rows = value; }
        }

        public void Accept (IMetadataTableVisitor visitor)
        {
            visitor.Visit (this);
            this.Rows.Accept (visitor.GetRowVisitor ());
        }
    }

    internal sealed class ClassLayoutRow : IMetadataRow {

        public static readonly int RowSize = 10;
        public static readonly int RowColumns = 3;

        private ushort m_packingSize;
        private uint m_classSize;
        private uint m_parent;

        public ushort PackingSize {
            get { return m_packingSize; }
            set { m_packingSize = value; }
        }

        public uint ClassSize {
            get { return m_classSize; }
            set { m_classSize = value; }
        }

        public uint Parent {
            get { return m_parent; }
            set { m_parent = value; }
        }

        public int Size {
            get { return ClassLayoutRow.RowSize; }
        }

        public int Columns {
            get { return ClassLayoutRow.RowColumns; }
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
