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

    [RId(0x0a)]
    internal sealed class MemberRefTable : IMetadataTable {

        private RowCollection m_rows;

        public MemberRefRow this[int index] {
            get { return m_rows[index] as MemberRefRow; }
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

    internal sealed class MemberRefRow : IMetadataRow {

        public static readonly int RowSize = 12;
        public static readonly int RowColumns = 3;

        [Column] private MetadataToken m_class;
        [Column] private uint m_name;
        [Column] private uint m_signature;

        public MetadataToken Class {
            get { return m_class; }
            set { m_class = value; }
        }

        public uint Name {
            get { return m_name; }
            set { m_name = value; }
        }

        public uint Signature {
            get { return m_signature; }
            set { m_signature = value; }
        }

        public int Size {
            get { return MemberRefRow.RowSize; }
        }

        public int Columns {
            get { return MemberRefRow.RowColumns; }
        }

        public void Accept(IMetadataRowVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
