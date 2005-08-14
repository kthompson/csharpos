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

namespace Mono.Cecil {

	public interface IArrayType : ITypeReference {
		IArrayDimensionCollection Dimensions { get; }
		ITypeReference ElementType { get; set; }
		int Rank { get; }
		bool IsSizedArray { get; }

		IArrayDimension DefineDimension (int lowerBound, int upperBound);
		IArrayDimension DefineDimension ();
	}
}
