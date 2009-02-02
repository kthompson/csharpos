using System;
using System.IO;
using Indy.IL2CPU.Assembler;


using CPU = Indy.IL2CPU.Assembler.X86;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace Indy.IL2CPU.IL.X86
{
    [OpCode(Mono.Cecil.Cil.Code.Ldloc)]
    public class Ldloc : Op
    {
        private MethodInformation.Variable _local;
        protected void SetLocalIndex(int aIndex, MethodInformation method)
        {
            _local = method.Locals[aIndex];
        }
        public Ldloc(MethodInformation method, int aIndex)
            : base(null, method)
        {
            SetLocalIndex(aIndex, method);
        }

        public Ldloc(Mono.Cecil.Cil.Instruction instruction, MethodInformation method)
            : base(instruction, method)
        {
            SetLocalIndex((int)instruction.Operand, method);
            //VariableDefinition xVarDef = instruction.Operand as VariableDefinition;
            //if (xVarDef != null) {
            //    SetLocalIndex(xVarDef.Index, aMethodInfo);
            //}
        }

        public sealed override void DoAssemble()
        {
            Ldloc(Assembler, _local);
        }
    }
}