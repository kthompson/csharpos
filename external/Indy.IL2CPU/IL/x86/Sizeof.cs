using System;
using System.Collections.Generic;
using System.IO;


using CPU = Indy.IL2CPU.Assembler.X86;
using Indy.IL2CPU.Assembler;
using Mono.Cecil;

namespace Indy.IL2CPU.IL.X86
{
    [OpCode(OpCodeEnum.Sizeof)]
    public class Sizeof : Op
    {
        private uint mTheSize;
        public static void ScanOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo, SortedList<string, object> aMethodData)
        {
            var typeRef = instruction.Operand as TypeReference;
            if (typeRef == null)
            {
                throw new Exception("Type not found!");
            }
            Engine.RegisterType(typeRef);
        }
        public Sizeof(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            var typeRef = instruction.Operand as TypeReference;
            if (typeRef == null)
            {
                throw new Exception("Type not found!");
            }
            Engine.GetTypeFieldInfo(typeRef, out mTheSize);
        }
        public override void DoAssemble()
        {
            new CPU.Push { DestinationValue = mTheSize };
            Assembler.StackContents.Push(new StackContent(4, typeof(int)));
        }
    }
}