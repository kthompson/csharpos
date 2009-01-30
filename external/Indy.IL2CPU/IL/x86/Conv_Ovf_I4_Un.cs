using System;
using System.IO;


using CPU = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Conv_Ovf_I4_Un)]
	public class Conv_Ovf_I4_Un: Op {
		public Conv_Ovf_I4_Un(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
		}
		public override void DoAssemble() {
			throw new NotImplementedException("This file has been autogenerated and not been changed afterwards!");
		}
	}
}