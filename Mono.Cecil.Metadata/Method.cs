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

	using Mono.Cecil.Binary;

	[RId (0x06)]
	public sealed class MethodTable : IMetadataTable {

		private RowCollection m_rows;

		public MethodRow this [int index] {
			get { return m_rows [index] as MethodRow; }
			set { m_rows [index] = value; }
		}

		public RowCollection Rows {
			get { return m_rows; }
			set { m_rows = value; }
		}

		internal MethodTable ()
		{
		}

		public void Accept (IMetadataTableVisitor visitor)
		{
			visitor.VisitMethodTable (this);
			this.Rows.Accept (visitor.GetRowVisitor ());
		}
	}

	public sealed class MethodRow : IMetadataRow {

		public static readonly int RowSize = 20;
		public static readonly int RowColumns = 6;

		public RVA RVA;
		public MethodImplAttributes ImplFlags;
		public MethodAttributes Flags;
		public uint Name;
		public uint Signature;
		public uint ParamList;

		internal MethodRow ()
		{
		}

		public void Accept (IMetadataRowVisitor visitor)
		{
			visitor.VisitMethodRow (this);
		}
	}
}
