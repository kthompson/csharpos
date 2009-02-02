using System;
using System.IO;


using CPU = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
    [OpCode(Mono.Cecil.Cil.Code.Mul_Ovf)]
	public class Mul_Ovf: Mul {
		public Mul_Ovf(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
		}

		public override void DoAssemble()
		{
			throw new NotImplementedException();
			//base.DoAssemble();
		}
	}
}