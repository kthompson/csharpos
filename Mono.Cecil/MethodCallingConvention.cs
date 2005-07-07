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

	using System;

	[Flags]
	public enum MethodCallingConvention : byte {
		Default		= 0x0,
		C			= 0x1,
		StdCall		= 0x2,
		ThisCall	= 0x3,
		FastCall	= 0x4,
		VarArg		= 0x5
	}
}
