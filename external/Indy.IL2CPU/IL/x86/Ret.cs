using System;
using System.IO;


using CPU = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(Mono.Cecil.Cil.Code.Ret)]
	public class Ret: Op {
		public Ret(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
		}
		public override void DoAssemble() {
            new CPU.Jump { DestinationLabel = MethodFooterOp.EndOfMethodLabelNameNormal };
		}
	}
}