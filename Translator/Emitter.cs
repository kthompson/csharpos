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

        public Emitter(TextWriter writer)
        {
            this._out = writer;
        }

        public Emitter()
            : this(new StringWriter())
        {
        }

        private void Emit(string format, params object[] args) 
        {
            _out.WriteLine(format, args);
        }

        #region ICodeVisitor Members

        public void VisitMethodBody(MethodBody body)
        {
            var name = body.Method.Name;
            Emit(".globl _{0}", name);
            Emit("  .def	_{0};	.scl	2;	.type	32;	.endef", name);
            Emit("_{0}:", name);
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
                    EmitLoadConstant((int)instr.Operand);
                    break;

                case Code.Ret:
                    Emit("ret");
                    break;

                default:
                    Assert.Break();
                    break;
            }
        }

        private void EmitLoadConstant(int value)
        {
            Emit("movl ${0}, %eax", value);
        }

        public void VisitMacroInstruction(Instruction instr)
        {
            switch (instr.OpCode.Code)
            {
                case Code.Ldc_I4_0:
                    EmitLoadConstant(0);
                    break;
                case Code.Ldc_I4_1:
                    EmitLoadConstant(1);
                    break;
                case Code.Ldc_I4_2:
                    EmitLoadConstant(2);
                    break;
                case Code.Ldc_I4_3:
                    EmitLoadConstant(3);
                    break;
                case Code.Ldc_I4_4:
                    EmitLoadConstant(4);
                    break;
                case Code.Ldc_I4_5:
                    EmitLoadConstant(5);
                    break;
                case Code.Ldc_I4_6:
                    EmitLoadConstant(6);
                    break;
                case Code.Ldc_I4_7:
                    EmitLoadConstant(7);
                    break;
                case Code.Ldc_I4_8:
                    EmitLoadConstant(8);
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
            
        }

        #endregion
    }
}
