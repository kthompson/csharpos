using System;
using System.IO;
using Indy.IL2CPU.Assembler;


using CPU = Indy.IL2CPU.Assembler.X86;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU.IL.X86
{
    [OpCode(OpCodeEnum.Ldloc)]
    public class Ldloc : Op
    {
        private VariableReference mLocal;
        protected void SetLocalIndex(int aIndex, MethodInformation aMethodInfo)
        {
            mLocal = aMethodInfo.Locals[aIndex];
        }
        public Ldloc(MethodInformation aMethodInfo, int aIndex)
            : base(null, aMethodInfo)
        {
            SetLocalIndex(aIndex, aMethodInfo);
        }

        public Ldloc(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            SetLocalIndex((int)instruction.Operand, aMethodInfo);
            //VariableDefinition xVarDef = instruction.Operand as VariableDefinition;
            //if (xVarDef != null) {
            //    SetLocalIndex(xVarDef.Index, aMethodInfo);
            //}
        }

        public sealed override void DoAssemble()
        {
            Ldloc(Assembler, mLocal);
        }
    }
}