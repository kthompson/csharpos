using System;

using CPUx86 = Indy.IL2CPU.Assembler.X86;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(Mono.Cecil.Cil.Code.Dup)]
	public class Dup: Op {
        public Dup(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
		}
		public override void DoAssemble() {
			var xStackContent = Assembler.StackContents.Peek();
			for (int i = 0; i < ((xStackContent.Size / 4) + (xStackContent.Size % 4 == 0 ? 0 : 1)) ; i++) {
                new CPUx86.Move { DestinationReg = CPUx86.Registers.EAX, SourceReg = CPUx86.Registers.ESP, SourceDisplacement=i*4, SourceIsIndirect=true };
                new CPUx86.Push { DestinationReg = CPUx86.Registers.EAX };
			}
			Assembler.StackContents.Push(xStackContent);
		}
	}
}