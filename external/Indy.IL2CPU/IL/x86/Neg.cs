using System;

using CPUx86 = Indy.IL2CPU.Assembler.X86;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(Mono.Cecil.Cil.Code.Neg)]
	public class Neg: Op {
        public Neg(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
		}
		public override void DoAssemble() {
			new CPUx86.Pop{DestinationReg=CPUx86.Registers.EAX};
			new CPUx86.Neg{DestinationReg=CPUx86.Registers.EAX};
            new CPUx86.Push { DestinationReg = CPUx86.Registers.EAX };
		}
	}
}