using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mono.Cecil.Cil;

namespace Translator
{
    public class Emitter : IEmitter
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

        public void VisitMethodBody(MethodBody body)
        {
            var name = body.Method.Name;
            this.Text.Emit(".globl _{0}", name);
            this.Text.Emit("\t.def\t_{0};\t.scl\t2;\t.type\t32;\t.endef", name);
            this.Text.Emit("_{0}:", name);
        }

        public void VisitInstructionCollection(InstructionCollection instructions)
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
                    Assert.Break();
                    break;
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

        public void VisitMacroInstruction(Instruction instr)
        {
            switch (instr.OpCode.Code)
            {
                case Code.Ldc_I4_M1:
                    EmitLoadConstantI4(-1);
                    break;
                case Code.Ldc_I4_0:
                    EmitLoadConstantI4(0);
                    break;
                case Code.Ldc_I4_1:
                    EmitLoadConstantI4(1);
                    break;
                case Code.Ldc_I4_2:
                    EmitLoadConstantI4(2);
                    break;
                case Code.Ldc_I4_3:
                    EmitLoadConstantI4(3);
                    break;
                case Code.Ldc_I4_4:
                    EmitLoadConstantI4(4);
                    break;
                case Code.Ldc_I4_5:
                    EmitLoadConstantI4(5);
                    break;
                case Code.Ldc_I4_6:
                    EmitLoadConstantI4(6);
                    break;
                case Code.Ldc_I4_7:
                    EmitLoadConstantI4(7);
                    break;
                case Code.Ldc_I4_8:
                    EmitLoadConstantI4(8);
                    break;
            }
        }

        public void VisitInstruction(Instruction instr)
        {
            switch(instr.OpCode.OpCodeType)
            {
                case OpCodeType.Annotation:
                
                case OpCodeType.Nternal:
                case OpCodeType.Objmodel:
                case OpCodeType.Prefix:
                    Assert.Break();
                    break;
                case OpCodeType.Macro:
                    this.VisitMacroInstruction(instr);
                    break;
                case OpCodeType.Primitive:
                    this.VisitPrimitiveInstruction(instr);
                    break;
            }
        }

        public void VisitExceptionHandlerCollection(ExceptionHandlerCollection seh)
        {
            throw new NotImplementedException();
        }

        public void VisitExceptionHandler(ExceptionHandler eh)
        {
            throw new NotImplementedException();
        }

        public void VisitVariableDefinitionCollection(VariableDefinitionCollection variables)
        {
            throw new NotImplementedException();
        }

        public void VisitVariableDefinition(VariableDefinition var)
        {
            throw new NotImplementedException();
        }

        public void VisitScopeCollection(ScopeCollection scopes)
        {
            throw new NotImplementedException();
        }

        public void VisitScope(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void TerminateMethodBody(MethodBody body)
        {
            if (this._sections.ContainsKey(SectionType.ReadOnlyData))
                this._sections[SectionType.ReadOnlyData].Flush(_out);

            if (this._sections.ContainsKey(SectionType.Text))
                this._sections[SectionType.Text].Flush(_out);
        }

        #endregion
    }
}
