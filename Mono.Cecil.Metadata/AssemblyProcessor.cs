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
 * Tue Jul 05 15:48:30 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

	[RId (0x21)]
	public sealed class AssemblyProcessorTable : IMetadataTable {

		private RowCollection m_rows;

		public AssemblyProcessorRow this [int index] {
			get { return m_rows [index] as AssemblyProcessorRow; }
			set { m_rows [index] = value; }
		}

		public RowCollection Rows {
			get { return m_rows; }
			set { m_rows = value; }
		}

		internal AssemblyProcessorTable ()
		{
		}

		public void Accept (IMetadataTableVisitor visitor)
		{
			visitor.Visit (this);
			this.Rows.Accept (visitor.GetRowVisitor ());
		}
	}

	public sealed class AssemblyProcessorRow : IMetadataRow {

		public static readonly int RowSize = 4;
		public static readonly int RowColumns = 1;

		public uint Processor;

		internal AssemblyProcessorRow ()
		{
		}

		public void Accept (IMetadataRowVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
