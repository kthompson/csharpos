using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cecil.Decompiler.Ast;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Cecil.Decompiler;

namespace Compiler
{
    public class Emitter 
    {
        private TextWriter _out;
        private readonly Dictionary<string, int> _variableLocations = new Dictionary<string,int>();
        private int _stackIndex;

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
            this._variableLocations.Clear();
            this._stackIndex = 0;
            var name = method.Name;
            this.Text.Emit(".globl _{0}", name);
            this.Text.Emit("\t.def\t_{0};\t.scl\t2;\t.type\t32;\t.endef", name);
            this.Text.Emit("_{0}:", name);
            this.VisitMethodBody(method.Body);
            this.TerminateMethodBody(method.Body);
        }

        public void VisitMethodBody(MethodBody body)
        {
            if (body.HasVariables)
            {
                foreach (VariableDefinition variable in body.Variables)
                {
                    _stackIndex -= 4;
                    _variableLocations.Add(variable.Name, _stackIndex);
                }
            }
            var block = body.Decompile();
            EmitBlockStatement(block, _stackIndex);
        }

        public void TerminateMethodBody(MethodBody body)
        {
            if (this._sections.ContainsKey(SectionType.ReadOnlyData))
                this._sections[SectionType.ReadOnlyData].Flush(_out);

            if (this._sections.ContainsKey(SectionType.Text))
                this._sections[SectionType.Text].Flush(_out);
        }

        public void EmitBlockStatement(BlockStatement node, int si)
        {
            Emit(node.Statements, si);
        }

        public void Emit(IEnumerable collection, int si)
        {
            foreach (ICodeNode node in collection)
                Emit(node, si);
        }

        public void Emit(ICodeNode node, int si)
        {
            if (node == null)
                return;

            if(node is Expression)
                EmitExpression((Expression)node, si);
            else if(node is Statement)
                EmitStatement((Statement)node, si);
            else
                Helper.Break();
        }

        public void EmitStatement(Statement node, int si)
        {
            switch (node.CodeNodeType)
            {
                case CodeNodeType.ReturnStatement:
                    EmitReturnStatement((ReturnStatement)node, si);
                    break;
                case CodeNodeType.ExpressionStatement:
                    EmitExpressionStatement((ExpressionStatement)node, si);
                    break;
                default:
                    Helper.NotSupported();
                    break;
            }
        }

        public void EmitExpressionStatement(ExpressionStatement node, int si)
        {
            EmitExpression(node.Expression, si);
        }

        public void EmitExpression(Expression node, int si)
        {
            this.Text.Emit("#" + node.ToCodeString());

            switch (node.CodeNodeType)
            {
                case CodeNodeType.AssignExpression:
                    EmitAssignExpression((AssignExpression)node, si);
                    break;
                case CodeNodeType.BinaryExpression:
                    EmitBinaryExpression((BinaryExpression)node, si);
                    break;
                case CodeNodeType.LiteralExpression:
                    EmitLiteralExpression((LiteralExpression)node, si);
                    break;
                case CodeNodeType.VariableReferenceExpression:
                    EmitVariableReferenceExpression((VariableReferenceExpression)node, si);
                    break;
                default:
                    Helper.NotSupported();
                    break;
            }
        }

        public void EmitReturnStatement(ReturnStatement node, int si)
        {
            Emit(node.Expression, si);
            this.Text.Emit(X86.OpCodes.Return.Create());
        }

        public void EmitLiteralExpression(LiteralExpression node, int si)
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

        public void EmitUnaryExpression(UnaryExpression node, int si)
        {
            switch (node.Operator)
            {
                case UnaryOperator.BitwiseNot:
                case UnaryOperator.LogicalNot:
                case UnaryOperator.Negate:
                default:
                    Helper.NotSupported();
                    break;
            }
        }

        public void EmitAssignExpression(AssignExpression node, int si)
        {
            EmitExpression(node.Expression, si);
            this.Text.Emit("movl %eax, {0}(%esp)", LookupVariable(node.Target));
        }

        private int LookupVariable(Expression node)
        {
            var var = node as VariableReferenceExpression;
            if (var == null)
                Helper.NotSupported();
            
            return _variableLocations[var.Variable.Name];
        }

       

        public void EmitBinaryExpression(BinaryExpression node, int si)
        {
            EmitExpression(node.Right, si);
            this.Text.Emit("movl %eax, {0}(%esp)", si);
            EmitExpression(node.Left, si - 4);

            switch (node.Operator)
            {
                case BinaryOperator.Add:
                    this.Text.Emit("addl {0}(%esp), %eax", si);
                    break;
                case BinaryOperator.Subtract:
                    this.Text.Emit("subl {0}(%esp), %eax", si);
                    break;
                case BinaryOperator.BitwiseAnd:
                    this.Text.Emit("andl {0}(%esp), %eax", si);
                    break;
                case BinaryOperator.BitwiseOr:
                    this.Text.Emit("orl {0}(%esp), %eax", si);
                    break;
                case BinaryOperator.BitwiseXor:
                    this.Text.Emit("xorl {0}(%esp), %eax", si);
                    break;
                case BinaryOperator.Multiply:
                    //this.Text.Emit("imull {0}(%esp)", si);
                    //break;
                case BinaryOperator.Divide:
                    //this.Text.Emit("idivl {0}(%esp)", si);
                    //break;
                default:
                    Helper.NotSupported();
                    break;
            }
        }

        public void EmitVariableReferenceExpression(VariableReferenceExpression node, int si)
        {
            this.Text.Emit("movl {0}(%esp), %eax", this._variableLocations[node.Variable.Name]);
        }

      
    }
}


