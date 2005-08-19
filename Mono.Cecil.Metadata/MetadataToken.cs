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
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

	public struct MetadataToken {

		private uint m_rid;
		private TokenType m_type;

		public uint RID {
			get { return m_rid; }
		}

		public TokenType TokenType {
			get { return m_type; }
		}

		public MetadataToken (TokenType table, uint rid)
		{
			m_type = table;
			m_rid = rid;
		}

		internal static MetadataToken FromMetadataRow (TokenType table, int rowIndex)
		{
			return new MetadataToken (table, (uint) rowIndex + 1);
		}

		public override string ToString ()
		{
			return string.Format ("{0} [0x{1}]",
				m_type, m_rid.ToString ("x4"));
		}
	}
}
