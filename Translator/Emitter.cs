using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil.Cil;

namespace Compiler
{
    public class Emitter : BaseCodeVisitor, IEmitter
    {
        private TextWriter _out;

        public Emitter()
            : this(new StringWriter())
        {
        }

        public Emitter(TextWriter writer)
        {
            _out = writer;
        }

        private Dictionary<SectionType, Section> _sections = new Dictionary<SectionType, Section>();
        protected Section Section(SectionType type)
        {
            if(!_sections.ContainsKey(type))
            {
                _sections.Add(type, new Section(type));
            }

            return _sections[type];
        }

        protected Section Text
        {
            get
            {
                return Section(SectionType.Text);
            }
        }

        protected Section ROData
        {
            get
            {
                return Section(SectionType.ReadOnlyData);
            }
        }

        #region ICodeVisitor Members

        public override void VisitMethodBody(MethodBody body)
        {
            var name = body.Method.Name;
            this.Text.Emit(".globl _{0}", name);
            this.Text.Emit("\t.def\t_{0};\t.scl\t2;\t.type\t32;\t.endef", name);
            this.Text.Emit("_{0}:", name);
        }

        public override void VisitInstructionCollection(InstructionCollection instructions)
        {
            foreach (var instruction in instructions)
                this.VisitInstruction(instruction);
        }

        public void VisitPrimitiveInstruction(Instruction instr)
        {
            switch (instr.OpCode.Code)
            {
                case Code.Ldc_I4:
                    EmitLoadConstantI4((int)instr.Operand);
                    break;
                case Code.Ldc_I8:
                    EmitLoadConstantI8((long)instr.Operand);
                    break;
                case Code.Ldc_R4:
                    EmitLoadConstantR4((float)instr.Operand);
                    break;
                case Code.Ldc_R8:
                    EmitLoadConstantR8((double)instr.Operand);
                    break;
                case Code.Ret:
                    this.Text.EmitReturn();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void EmitLoadConstantI4(int value)
        {
            this.Text.EmitMoveImmediate(value.ToString(), "eax");
        }

        private void EmitLoadConstantI8(long value)
        {
            //split into upper 8 and lower 8
            long upper = (value) >> 32;
            long lower = (value) & 0xffffffff;
            this.Text.EmitMoveImmediate(upper.ToString(), "eax");
            this.Text.EmitMoveImmediate(lower.ToString(), "edx");
            throw new NotImplementedException();
        }

        private void EmitLoadConstantR4(float value)
        {
            /*
              	.section .rdata,"dr"
	            .align 4
            LC0:
	            .long	1078523331 
            	.text
            .globl _scheme_entry
	            .def	_scheme_entry;	.scl	2;	.type	32;	.endef
            _scheme_entry:
	            flds	LC0
	            ret
             */
            var label = this.ROData.Label(section => section.EmitLong(value.ToIEEE754()));
            this.Text.Emit("\tflds\t{0}", label);
        }

        private void EmitLoadConstantR8(double value)
        {
            /*
            	.section .rdata,"dr"
            	.align 8
            LC0:
            	.long	1374389535
            	.long	1074339512
            	.text
            	.p2align 4,,15
            .globl _scheme_entry
            	.def	_scheme_entry;	.scl	2;	.type	32;	.endef
            _scheme_entry:
            	fldl	LC0
            	ret
             */
            throw new NotImplementedException();
            //Emit("movl ${0}, %eax", value);
        }

        public override void VisitInstruction(IInstruction instr)
        {
            var instruction = instr as Instruction;
            if (instruction != null)
            {
                Helper.Break();
                return;
            }

            this.Text.Emit(instr.ToString());
        }

        public override void TerminateMethodBody(MethodBody body)
        {
            if (this._sections.ContainsKey(SectionType.ReadOnlyData))
                this._sections[SectionType.ReadOnlyData].Flush(_out);

            if (this._sections.ContainsKey(SectionType.Text))
                this._sections[SectionType.Text].Flush(_out);
        }

        #endregion
    }
}


