using System;
using System.Collections.Generic;
using System.IO;


using CPU = Indy.IL2CPU.Assembler.X86;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU.IL.X86 {
    [OpCode(Code.Ldelem_I1)]
	public class Ldelem_I1: Op {
        public static void ScanOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo, SortedList<string, object> aMethodData)
        {
            Engine.RegisterType<sbyte>();
        }

        public Ldelem_I1(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
		}
		public override void DoAssemble() {
			Ldelem_Ref.Assemble(Assembler, 1);
		}
	}
}