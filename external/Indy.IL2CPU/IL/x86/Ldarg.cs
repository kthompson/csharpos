using System;
using System.Collections.Generic;
using System.IO;

using CPU = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Ldarg)]
	public class Ldarg: Op {
		private MethodInformation.Argument mArgument;
		protected void SetArgIndex(int aIndex, MethodInformation aMethodInfo) {
			mArgument = aMethodInfo.Arguments[aIndex];
		}

		public Ldarg(MethodInformation aMethodInfo, int aIndex)
			: base(null, aMethodInfo) {
			SetArgIndex(aIndex, aMethodInfo);
		}

		public Ldarg(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
			int xArgIndex;
            if (instruction != null)
            {
                xArgIndex = (int)instruction.Operand;
				SetArgIndex(xArgIndex, aMethodInfo);
				//ParameterDefinition xParam = instruction.Operand as ParameterDefinition;
				//if (xParam != null) {
				//    SetArgIndex(xParam.Sequence - 1, aMethodInfo);
				//}
			}
		}

		public override void DoAssemble() {
			Ldarg(Assembler, mArgument);
		}
	}
}