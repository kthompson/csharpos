using System;
using System.IO;


using CPU = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Br)]
	public class Br: Op {
		private readonly string mTargetInstructionName;
		public Br(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
			mTargetInstructionName = GetInstructionLabel((long)instruction.Operand);
		}
		public override void DoAssemble() {
            new CPU.Jump { DestinationLabel = mTargetInstructionName };
		}
	}
}