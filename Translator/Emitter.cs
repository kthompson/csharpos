using System;
using System.Collections.Generic;
using System.IO;
using Cecil.Decompiler.Ast;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Cecil.Decompiler;

namespace Compiler
{
    public class Emitter : Cecil.Decompiler.Ast.BaseCodeVisitor
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

        private readonly Dictionary<SectionType, Section> _sections = new Dictionary<SectionType, Section>();
        public virtual Section Section(SectionType type)
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

        public void VisitMethodDefinition(MethodDefinition method)
        {
            var name = method.Name;
            this.Text.Emit(".globl _{0}", name);
            this.Text.Emit("\t.def\t_{0};\t.scl\t2;\t.type\t32;\t.endef", name);
            this.Text.Emit("_{0}:", name);
            this.VisitMethodBody(method.Body);
            this.TerminateMethodBody(method.Body);
        }

        public void VisitMethodBody(MethodBody body)
        {
            var block = body.Decompile();
            this.Visit(block);
        }

        public void TerminateMethodBody(MethodBody body)
        {
            if (this._sections.ContainsKey(SectionType.ReadOnlyData))
                this._sections[SectionType.ReadOnlyData].Flush(_out);

            if (this._sections.ContainsKey(SectionType.Text))
                this._sections[SectionType.Text].Flush(_out);
        }

        public override void VisitReturnStatement(ReturnStatement node)
        {
            base.VisitReturnStatement(node);
            this.Text.Emit(X86.OpCodes.Return.Create());
        }

        public override void VisitLiteralExpression(LiteralExpression node)
        {
            if(node.Value is int)
            {
                this.Text.Emit(X86.OpCodes.Move.Create("$" + node.Value, "%eax"));
            }
            else if(node.Value is float)
            {
                var value = (float)node.Value;
                var label = this.ROData.Label(section => section.EmitLong(value.ToIEEE754()));
                this.Text.Emit(X86.OpCodes.LoadReal.Create(label));
            }
            else
            {
                Helper.NotSupported();
            }
        }

    }
}


