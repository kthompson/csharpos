using System;
using System.IO;


using CPU = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Stelem_R4)]
	public class Stelem_R4: Op {
		public Stelem_R4(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
		}
		public override void DoAssemble() {
			Stelem_Ref.Assemble(Assembler, 4);
		}
	}
}