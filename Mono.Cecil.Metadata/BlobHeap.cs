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

	using System.IO;

	public class BlobHeap : MetadataHeap {

		internal BlobHeap (MetadataStream stream) : base (stream, "#Blob")
		{
		}

		public byte [] Read (uint index)
		{
			return ReadBytesFromStream (index);
		}

		public BinaryReader GetReader (uint index)
		{
			return new BinaryReader (new MemoryStream (Read (index)));
		}

		public override void Accept (IMetadataVisitor visitor)
		{
			visitor.Visit (this);
		}
	}
}
