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
 * Wed Jan 19 14:28:45 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

    using Mono.Cecil;

    [RId (0x02)]
    internal sealed class TypeDefTable : IMetadataTable {

        private RowCollection m_rows;

        public TypeDefRow this [int index] {
            get { return m_rows [index] as TypeDefRow; }
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

    internal sealed class TypeDefRow : IMetadataRow {

        public static readonly int RowSize = 24;
        public static readonly int RowColumns = 6;

        [Column] private TypeAttributes m_flags;
        [Column] private uint m_name;
        [Column] private uint m_namespace;
        [Column] private MetadataToken m_extends;
        [Column] private uint m_fieldList;
        [Column] private uint m_methodList;

        public TypeAttributes Flags {
            get { return m_flags; }
            set { m_flags = value; }
        }

        public uint Name {
            get { return m_name; }
            set { m_name = value; }
        }

        public uint Namespace {
            get { return m_namespace; }
            set { m_namespace = value; }
        }

        public MetadataToken Extends {
            get { return m_extends; }
            set { m_extends = value; }
        }

        public uint FieldList {
            get { return m_fieldList; }
            set { m_fieldList = value; }
        }

        public uint MethodList {
            get { return m_methodList; }
            set { m_methodList = value; }
        }

        public int Size {
            get { return TypeDefRow.RowSize; }
        }

        public int Columns {
            get { return TypeDefRow.RowColumns; }
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
