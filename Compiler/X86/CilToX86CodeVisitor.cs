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
        private int _stackIndex = -4;
        private int _variableCount;

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
            _variableCount = body.Variables.Count;
        }

        public override void VisitInstructionCollection(InstructionCollection instructions)
        {
            for (var i = 0; i < instructions.Count; i++)
                this.VisitInstruction(instructions[i]);
        }

        //public override void VisitInstruction(IInstruction instr)
        //{
        //    var instruction = instr as Instruction;
        //    if (instruction != null)
        //        this.VisitInstruction(instruction);
        //}

        public override void VisitInstruction(Instruction instr)
        {
            switch (instr.OpCode.Code)
            {
                case Mono.Cecil.Cil.Code.Ldc_I4_S:
                case Mono.Cecil.Cil.Code.Ldc_I4:
                    LoadConstantI4(instr);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_M1:
                case Mono.Cecil.Cil.Code.Ldc_I4_0:
                case Mono.Cecil.Cil.Code.Ldc_I4_1:
                case Mono.Cecil.Cil.Code.Ldc_I4_2:
                case Mono.Cecil.Cil.Code.Ldc_I4_3:
                case Mono.Cecil.Cil.Code.Ldc_I4_4:
                case Mono.Cecil.Cil.Code.Ldc_I4_5:
                case Mono.Cecil.Cil.Code.Ldc_I4_6:
                case Mono.Cecil.Cil.Code.Ldc_I4_7:
                case Mono.Cecil.Cil.Code.Ldc_I4_8:
                    LoadConstantI4(instr, instr.OpCode.Value - 0x16);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_R4:
                    LoadConstantR4(instr);
                    break;
                case Mono.Cecil.Cil.Code.Ret:
                    throw new NotImplementedException();
                    //Replace(instr, new X86Instruction(OpCodes.Return));
                    break;
            }
        }

        private string ImmediateRepresentation(object x)
        {
            if(x is int)
                return string.Format("${0}", x);

            if (x is sbyte)
                return string.Format("${0}", Convert.ToInt16((sbyte)x));

            if(x is float)
            {
                var rodata = this.MethodCompiler.Emitter.Section(SectionType.ReadOnlyData);
                var label = rodata.Label(secton => secton.EmitLong(((float)x).ToIEEE754()));
                return label.ToString();
            }

            Helper.NotSupported();
            return null;
        }

        private void PrimitiveCall(Instruction instr)
        {
        }

        private void LoadConstantR4(Instruction instr)
        {
            //this.Replace(instr, new X86Instruction(OpCodes.LoadReal, ImmediateRepresentation(instr.Operand)));
        }

        private void LoadConstantI4(Instruction instr, object value = null)
        {
            //Replace(instr, new X86Instruction(OpCodes.Move, ImmediateRepresentation(value ?? instr.Operand), Registers.Eax));
        }

        //private void Replace(IInstruction original, IInstruction newInstruction)
        //{
        //    _worker.Replace(original, newInstruction);
        //}
    }
}
