using System;
using System.IO;


using CPU = Indy.IL2CPU.Assembler.X86;
using Indy.IL2CPU.Assembler;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(Mono.Cecil.Cil.Code.Ldnull)]
	public class Ldnull: Op {
		public Ldnull(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
		}
		public override void DoAssemble() {
            new CPU.Push { DestinationValue = 0 };
			Assembler.StackContents.Push(new StackContent(4, typeof(uint)));
		}
	}
}