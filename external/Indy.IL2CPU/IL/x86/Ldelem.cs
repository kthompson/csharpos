using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Indy.IL2CPU.IL.X86
{
    [OpCode(OpCodeEnum.Ldelem)]
    public class Ldelem : Op
    {
        private uint mElementSize;

        public static void ScanOp(ILReader aReader, MethodInformation aMethodInfo, SortedList<string, object> aMethodData)
        {
            var xType = aReader.OperandValueType;
            if (xType == null)
                throw new Exception("Unable to determine Type!");
            Engine.RegisterType(xType);
        }

        public Ldelem(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            var xType = (TypeReference)instruction.Operand;
            if (xType == null)
                throw new Exception("Unable to determine Type!");
            mElementSize = Engine.GetFieldStorageSize(xType);
        }

        public override void DoAssemble()
        {
            Ldelem_Ref.Assemble(Assembler, mElementSize);
        }
    }
}