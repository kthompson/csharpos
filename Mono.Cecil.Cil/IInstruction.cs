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

namespace Mono.Cecil.Cil {

	public interface IInstruction : ICodeVisitable {

		int Offset { get; }
		//ILabel Label { get; }
		OpCode OpCode { get; }
		object Operand { get; }
	}
}
