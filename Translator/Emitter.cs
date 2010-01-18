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
        public Section Section(SectionType type)
        {
            if(!_sections.ContainsKey(type))
                _sections.Add(type, new Section(type));

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

        //private void EmitLoadConstantI8(long value)
        //{
        //    //split into upper 8 and lower 8
        //    long upper = (value) >> 32;
        //    long lower = (value) & 0xffffffff;
        //    this.Text.EmitMoveImmediate(upper.ToString(), "eax");
        //    this.Text.EmitMoveImmediate(lower.ToString(), "edx");
        //    throw new NotImplementedException();
        //}

        public override void VisitInstruction(IInstruction instr)
        {
            var instruction = instr as Instruction;
            if (instruction != null)
            {
                Helper.NotSupported(instruction.OpCode.ToString());
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


