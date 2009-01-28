using System;
using System.Collections.Generic;
using CPU = Indy.IL2CPU.Assembler.X86;
using System.Reflection;
using Indy.IL2CPU.Assembler;
using Mono.Cecil;

namespace Indy.IL2CPU.IL.X86 {
	[OpCode(OpCodeEnum.Ldsflda)]
	public class Ldsflda: Op {
		private readonly string mDataName;
        public static void ScanOp(ILReader aReader, MethodInformation aMethodInfo, SortedList<string, object> aMethodData) {
            var field = aReader.OperandValueField;
            Engine.QueueStaticField(field);
        }

		public Ldsflda(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
			var xField = (FieldDefinition)instruction.Operand;
			Engine.QueueStaticField(xField, out mDataName);
		}

		public override void DoAssemble() {
            new CPU.Push { DestinationRef = new ElementReference(mDataName) };
			Assembler.StackContents.Push(new StackContent(4, true, false, false));
		}
	}
}