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
 * Mon Jan 10 00:16:44 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

    using Mono.Cecil.Metadata;

    [RId(0x0e)]
    internal sealed class DeclSecurityTable : IMetadataTable {

        private RowCollection m_rows;

        public DeclSecurityRow this[int index] {
            get { return m_rows[index] as DeclSecurityRow; }
            set { m_rows[index] = value; }
        }

        public RowCollection Rows {
            get { return m_rows; }
            set { m_rows = value; }
        }

        public void Accept(IMetadataTableVisitor visitor) {
            visitor.Visit(this);
            this.Rows.Accept(visitor.GetRowVisitor());
        }
    }

    internal sealed class DeclSecurityRow : IMetadataRow {

        public static readonly int RowSize = 10;
        public static readonly int RowColumns = 3;

        [Column] private short m_action;
        [Column] private MetadataToken m_parent;
        [Column] private uint m_permissionSet;

        public short Action {
            get { return m_action; }
            set { m_action = value; }
        }

        public MetadataToken Parent {
            get { return m_parent; }
            set { m_parent = value; }
        }

        public uint PermissionSet {
            get { return m_permissionSet; }
            set { m_permissionSet = value; }
        }

        public int Size {
            get { return DeclSecurityRow.RowSize; }
        }

        public int Columns {
            get { return DeclSecurityRow.RowColumns; }
        }

        public void Accept(IMetadataRowVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
