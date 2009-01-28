using System;

using CPUx86 = Indy.IL2CPU.Assembler.X86;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Ldind_R4)]
	public class Ldind_R4: Op {
        public Ldind_R4(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
		}
		public override void DoAssemble() {
			throw new Exception("Floats not supported yet");
			//Assembler.StackContents.Pop();
			//new CPUx86.Pop(CPUx86.Registers_Old.EAX);
			//new CPUx86.Pushd(CPUx86.Registers_Old.EAX);
			//Assembler.StackContents.Push(4);
		}
	}
}