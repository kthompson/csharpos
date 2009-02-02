using System;
using System.IO;


using CPU = Indy.IL2CPU.Assembler.X86;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU.IL.X86
{
    [OpCode(Mono.Cecil.Cil.Code.Leave)]
    public class Leave : Op
    {
        public readonly string TargetLabel;
        public Leave(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            TargetLabel = GetInstructionLabel((Instruction)instruction.Operand);
        }
        public override void DoAssemble()
        {
            new CPU.Jump { DestinationLabel = TargetLabel };
        }
    }
}