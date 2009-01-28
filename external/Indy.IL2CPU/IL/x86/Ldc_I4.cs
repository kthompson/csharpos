using System;
using System.Linq;
using Indy.IL2CPU.Assembler;
using CPU = Indy.IL2CPU.Assembler.X86;
using System.Collections.Generic;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Ldc_I4)]
	public class Ldc_I4: Op {
		private int mValue;
		protected void SetValue(int aValue) {
			mValue = aValue;
		}

		protected void SetValue(string aValue) {
			SetValue((Int32)UInt32.Parse(aValue));
		}

        public Ldc_I4(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            if (instruction != null)
            {
                SetValue((Int32)instruction.Operand);
            }
        }

		public int Value {
			get {
				return mValue;
			}
		}
		public override sealed void DoAssemble() {
            new CPU.Push { DestinationValue = (uint)mValue };
			Assembler.StackContents.Push(new StackContent(4, typeof(int)));
		}
	}
}