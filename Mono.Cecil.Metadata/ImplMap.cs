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

    using Mono.Cecil;

    [RId (0x1c)]
    internal sealed class ImplMapTable : IMetadataTable {

        private RowCollection m_rows;

        public ImplMapRow this [int index] {
            get { return m_rows [index] as ImplMapRow; }
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

    internal sealed class ImplMapRow : IMetadataRow {

        public static readonly int RowSize = 14;
        public static readonly int RowColumns = 4;

        private PInvokeAttributes m_mappingFlags;
        private MetadataToken m_memberForwarded;
        private uint m_importName;
        private uint m_importScope;

        public PInvokeAttributes MappingFlags {
            get { return m_mappingFlags; }
            set { m_mappingFlags = value; }
        }

        public MetadataToken MemberForwarded {
            get { return m_memberForwarded; }
            set { m_memberForwarded = value; }
        }

        public uint ImportName {
            get { return m_importName; }
            set { m_importName = value; }
        }

        public uint ImportScope {
            get { return m_importScope; }
            set { m_importScope = value; }
        }

        public int Size {
            get { return ImplMapRow.RowSize; }
        }

        public int Columns {
            get { return ImplMapRow.RowColumns; }
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
