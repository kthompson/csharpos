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

	[RId (0x23)]
	public sealed class AssemblyRefTable : IMetadataTable {

		private RowCollection m_rows;

		public AssemblyRefRow this [int index] {
			get { return m_rows [index] as AssemblyRefRow; }
			set { m_rows [index] = value; }
		}

		public RowCollection Rows {
			get { return m_rows; }
			set { m_rows = value; }
		}

		internal AssemblyRefTable ()
		{
		}

		public void Accept (IMetadataTableVisitor visitor)
		{
			visitor.VisitAssemblyRefTable (this);
			this.Rows.Accept (visitor.GetRowVisitor ());
		}
	}

	public sealed class AssemblyRefRow : IMetadataRow {

		public static readonly int RowSize = 28;
		public static readonly int RowColumns = 9;

		public ushort MajorVersion;
		public ushort MinorVersion;
		public ushort BuildNumber;
		public ushort RevisionNumber;
		public AssemblyFlags Flags;
		public uint PublicKeyOrToken;
		public uint Name;
		public uint Culture;
		public uint HashValue;

		internal AssemblyRefRow ()
		{
		}

		public void Accept (IMetadataRowVisitor visitor)
		{
			visitor.VisitAssemblyRefRow (this);
		}
	}
}
