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
 * Thu Feb 24 01:20:28 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

    using Mono.Cecil;

    [RId (0x17)]
    internal sealed class PropertyTable : IMetadataTable {

        private RowCollection m_rows;

        public PropertyRow this [int index] {
            get { return m_rows [index] as PropertyRow; }
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

    internal sealed class PropertyRow : IMetadataRow {

        public static readonly int RowSize = 10;
        public static readonly int RowColumns = 3;

        private PropertyAttributes m_flags;
        private uint m_name;
        private uint m_type;

        public PropertyAttributes Flags {
            get { return m_flags; }
            set { m_flags = value; }
        }

        public uint Name {
            get { return m_name; }
            set { m_name = value; }
        }

        public uint Type {
            get { return m_type; }
            set { m_type = value; }
        }

        public int Size {
            get { return PropertyRow.RowSize; }
        }

        public int Columns {
            get { return PropertyRow.RowColumns; }
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
