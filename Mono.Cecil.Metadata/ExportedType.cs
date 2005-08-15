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
 * Mon Aug 15 02:23:17 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

	using Mono.Cecil;

	[RId (0x27)]
	public sealed class ExportedTypeTable : IMetadataTable {

		private RowCollection m_rows;

		public ExportedTypeRow this [int index] {
			get { return m_rows [index] as ExportedTypeRow; }
			set { m_rows [index] = value; }
		}

		public RowCollection Rows {
			get { return m_rows; }
			set { m_rows = value; }
		}

		internal ExportedTypeTable ()
		{
		}

		public void Accept (IMetadataTableVisitor visitor)
		{
			visitor.VisitExportedTypeTable (this);
			this.Rows.Accept (visitor.GetRowVisitor ());
		}
	}

	public sealed class ExportedTypeRow : IMetadataRow {

		public static readonly int RowSize = 20;
		public static readonly int RowColumns = 5;

		public TypeAttributes Flags;
		public uint TypeDefId;
		public uint TypeName;
		public uint TypeNamespace;
		public MetadataToken Implementation;

		internal ExportedTypeRow ()
		{
		}

		public void Accept (IMetadataRowVisitor visitor)
		{
			visitor.VisitExportedTypeRow (this);
		}
	}
}
