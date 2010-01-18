using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;

namespace Compiler.X86
{
    public class CilToX86CodeVisitor : BaseCodeVisitor
    {
        public MethodCompiler MethodCompiler { get; set; }
        private CilWorker _worker;

        public CilToX86CodeVisitor(MethodCompiler methodCompiler)
        {
            this.MethodCompiler = methodCompiler;
        }

        public override void TerminateMethodBody(MethodBody body)
        {
            _worker = null;
        }

        public override void VisitMethodBody(MethodBody body)
        {
            _worker = body.CilWorker;
        }

        public override void VisitInstructionCollection(InstructionCollection instructions)
        {
            for (var i = 0; i < instructions.Count; i++)
                this.VisitInstruction(instructions[i]);
        }

        public override void VisitInstruction(IInstruction instr)
        {
            var instruction = instr as Instruction;
            if (instruction != null)
                this.VisitInstruction(instruction);
        }

        public override void VisitInstruction(Instruction instr)
        {
            switch(instr.OpCode.Code)
            {
                case Mono.Cecil.Cil.Code.Ldc_I4:
                    ReplaceLoadConstantI4(instr, (int)instr.Operand);
                    break;

                case Mono.Cecil.Cil.Code.Ldc_I4_M1:
                    ReplaceLoadConstantI4(instr,-1);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_0:
                    ReplaceLoadConstantI4(instr,0);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_1:
                    ReplaceLoadConstantI4(instr,1);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_2:
                    ReplaceLoadConstantI4(instr,2);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_3:
                    ReplaceLoadConstantI4(instr,3);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_4:
                    ReplaceLoadConstantI4(instr,4);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_5:
                    ReplaceLoadConstantI4(instr,5);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_6:
                    ReplaceLoadConstantI4(instr,6);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_7:
                    ReplaceLoadConstantI4(instr,7);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_8:
                    ReplaceLoadConstantI4(instr,8);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_S:
                    ReplaceLoadConstantI4(instr,Convert.ToInt16((sbyte)instr.Operand));
                    break;
                case Mono.Cecil.Cil.Code.Ldc_R4:
                    ReplaceLoadConstantR4(instr);
                    break;
                case Mono.Cecil.Cil.Code.Ret: 
                    Replace(instr, new X86Instruction(OpCodes.Return));
                    break;
            }
        }

        private void ReplaceLoadConstantR4(Instruction instr)
        {
            var value = (float)instr.Operand;
            var rodata = this.MethodCompiler.Emitter.Section(SectionType.ReadOnlyData);
            var label = rodata.Label(secton => secton.EmitLong(value.ToIEEE754()));
            this.Replace(instr, new X86Instruction(OpCodes.LoadReal, label));
        }

        private void ReplaceLoadConstantI4(Instruction instr, int value)
        {
            Replace(instr, new X86Instruction(OpCodes.Move, string.Format("${0}", value), Registers.Eax));
        }

        private void Replace(IInstruction original, IInstruction newInstruction)
        {
            _worker.Replace(original, newInstruction);
        }
    }
}
