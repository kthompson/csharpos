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
 * Tue Jul 05 15:48:31 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

	using Mono.Cecil;

	[RId (0x2a)]
	public sealed class GenericParamTable : IMetadataTable {

		private RowCollection m_rows;

		public GenericParamRow this [int index] {
			get { return m_rows [index] as GenericParamRow; }
			set { m_rows [index] = value; }
		}

		public RowCollection Rows {
			get { return m_rows; }
			set { m_rows = value; }
		}

		internal GenericParamTable ()
		{
		}

		public void Accept (IMetadataTableVisitor visitor)
		{
			visitor.Visit (this);
			this.Rows.Accept (visitor.GetRowVisitor ());
		}
	}

	public sealed class GenericParamRow : IMetadataRow {

		public static readonly int RowSize = 12;
		public static readonly int RowColumns = 4;

		public ushort Number;
		public GenericParamAttributes Flags;
		public MetadataToken Owner;
		public uint Name;

		internal GenericParamRow ()
		{
		}

		public void Accept (IMetadataRowVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
