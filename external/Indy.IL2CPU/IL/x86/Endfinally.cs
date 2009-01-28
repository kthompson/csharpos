using System;
using System.IO;


using CPU = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Endfinally)]
	public class Endfinally: Op {
		public Endfinally(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
		}
		public override void DoAssemble() {
			// TODO: unimplemented
			//throw new NotImplementedException("This file has been autogenerated and not been changed afterwards!");
		}
	}
}