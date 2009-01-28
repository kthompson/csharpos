using System;
using System.IO;
using Indy.IL2CPU.IL;
using CPU = Indy.IL2CPU.Assembler.X86;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU.IL.X86 {
    [OpCode(OpCodeEnum.Nop)]
    public class Nop : Op {
        public Nop(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
		}
		public override void DoAssemble() {
            // Assembler would be base type in IL
            // Cast to ours
            // var x = (X86.Assembler)Assembler
            // This would solve the threading issue
            // and later allow for operator overloads etc.
            // x.Noop();
            
            // For now we dont transalte NOOP's at all. Outputting them gives us nothing when coming from 
            // IL.
			new CPU.Noop();
		}
	}
}