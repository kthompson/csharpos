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
 * Sat May 14 20:51:44 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

    [RId (0x1a)]
    public sealed class ModuleRefTable : IMetadataTable {

        private RowCollection m_rows;

        public ModuleRefRow this [int index] {
            get { return m_rows [index] as ModuleRefRow; }
            set { m_rows [index] = value; }
        }

        public RowCollection Rows {
            get { return m_rows; }
            set { m_rows = value; }
        }

        internal ModuleRefTable ()
        {
        }

        public void Accept (IMetadataTableVisitor visitor)
        {
            visitor.Visit (this);
            this.Rows.Accept (visitor.GetRowVisitor ());
        }
    }

    public sealed class ModuleRefRow : IMetadataRow {

        public static readonly int RowSize = 4;
        public static readonly int RowColumns = 1;

        public uint Name;

        internal ModuleRefRow ()
        {
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
