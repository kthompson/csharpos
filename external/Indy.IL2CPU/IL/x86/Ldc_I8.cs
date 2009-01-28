using System;
using System.IO;
using CPU = Indy.IL2CPU.Assembler.X86;
using Indy.IL2CPU.Assembler;
using System.Diagnostics;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Ldc_I8)]
	public class Ldc_I8: Op {
		private readonly long mValue;
		public Ldc_I8(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo)
		{
            mValue = (long)instruction.Operand;
		}
		public override void DoAssemble() {
			string theValue = mValue.ToString("X16");
            new CPU.Push { DestinationValue = BitConverter.ToUInt32(BitConverter.GetBytes(mValue), 0) };
            new CPU.Push { DestinationValue = BitConverter.ToUInt32(BitConverter.GetBytes(mValue), 4) };
			Assembler.StackContents.Push(new StackContent(8, typeof(long)));
		}
	}
}