using System;
using System.Collections.Generic;
using System.IO;


using CPU = Indy.IL2CPU.Assembler;
using CPUx86 = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Ldelem_I4)]
	public class Ldelem_I4: Op {
        public static void ScanOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo, SortedList<string, object> aMethodData)
        {
            Engine.RegisterType<int>();
        }

        public Ldelem_I4(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
		}

		public override void DoAssemble() {
			Ldelem_Ref.Assemble(Assembler, 4);
		}
	}
}