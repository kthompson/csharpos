using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Indy.IL2CPU.IL.X86
{
    [OpCode(Mono.Cecil.Cil.Code.Stelem_Any)]
    public class Stelem : Op
    {
        private uint mElementSize;
        public Stelem(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            var xType = instruction.Operand as TypeReference;
            if (xType == null)
                throw new Exception("Unable to determine Type!");
            mElementSize = Engine.GetFieldStorageSize(xType);
        }

        public override void DoAssemble()
        {
            Stelem_Ref.Assemble(Assembler, mElementSize);
        }
    }
}