using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Cecil.Decompiler.Ast;
using Compiler.Ast;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Cecil.Decompiler;

namespace Compiler
{
    public class Emitter 
    {
        private TextWriter _out;
        private readonly Dictionary<string, int> _variableLocations = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _parameterLocations = new Dictionary<string, int>();
        private int _stackIndex;

        public Emitter()
            : this(new StringWriter())
        {
        }

        public Emitter(TextWriter writer)
        {
            _out = writer;
        }

        private int _labelCount;
        protected string GetUniqueLabel()
        {
            return string.Format("L_{0}", _labelCount++);
        }

        private readonly Dictionary<string, string> _mappedLabels = new Dictionary<string, string>();
        protected string GetMappedLabel(string ilLabel)
        {
            if (!_mappedLabels.ContainsKey(ilLabel))
                _mappedLabels.Add(ilLabel, GetUniqueLabel());

            return _mappedLabels[ilLabel];
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
            //cleanup
            this._variableLocations.Clear();
            this._stackIndex = 0;
            this._labelCount = 0;

            var name = method.Name;
            this.Text.Emit(".globl _{0}", name);
            this.Text.Emit("\t.def\t_{0};\t.scl\t2;\t.type\t32;\t.endef", name);
            this.Text.Emit("_{0}:", name);
            this.VisitMethodBody(method.Body);
            this.TerminateMethodBody(method.Body);
        }

        protected void VisitMethodBody(MethodBody body)
        {
            var localVariableSize = 0;
            if (body.HasVariables)
            {
                foreach (VariableDefinition variable in body.Variables)
                {
                    localVariableSize += GetVariableSize(variable);
                    _variableLocations.Add(variable.Name, -localVariableSize);
                }
            }

            if (body.Method.HasParameters)
            {
                var size = 4; //skip return point
                foreach (ParameterDefinition item in body.Method.Parameters)
                {
                    size += 4;
                    _parameterLocations.Add(item.Name, size);
                }
            }

            var block = body.Decompile();
            block = (BlockStatement)new TypedTransformer().VisitBlockStatement(block);
            EmitMethodEntry(localVariableSize);
            EmitBlockStatement(block, -localVariableSize);
        }

        private void EmitMethodEntry(int localVariableSize)
        {
            this.Text.Emit("pushl	%ebp");
            this.Text.Emit("movl	%esp, %ebp");
            if (localVariableSize > 0)
                this.Text.Emit("subl	${0}, %esp", localVariableSize);
        }

        private int GetVariableSize(VariableDefinition variable)
        {
            return 4;
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

        private void EmitComparePattern(string left, string right, Action then, Action @else)
        {
            EmitComparePattern(left, right, then, @else, "je");
        }

        private void EmitComparePattern(string left, string right, Action then, Action @else, string jmpType)
        {
            EmitBranchPattern(() =>
                              this.Text.Emit("cmp {0}, {1}", left, right),
                              @else,
                              then, jmpType);
        }

        private void EmitBranchPattern(Action test, Action @else, Action then)
        {
            EmitBranchPattern(test, @else, then, "je");
        }

        private void EmitBranchPattern(Action test, Action @else, Action then, string jmpType)
        {
            var altLabel = GetUniqueLabel();
            var endLabel = GetUniqueLabel();
            test();
            this.Text.Emit("{0} {1}",jmpType, altLabel);
            @else();
            this.Text.Emit("jmp {0}", endLabel);
            this.Text.Emit("{0}:", altLabel);
            then();
            this.Text.Emit("{0}:", endLabel);
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
                case CodeNodeType.ArgumentReferenceExpression:
                    EmitArgumentReferenceExpression((ArgumentReferenceExpression)node, si);
                    break;
                case CodeNodeType.BinaryExpression:
                    EmitBinaryExpression((TypedBinaryExpression)node, si);
                    break;
                case CodeNodeType.LiteralExpression:
                    EmitLiteralExpression((LiteralExpression)node, si);
                    break;
                case CodeNodeType.VariableReferenceExpression:
                    EmitVariableReferenceExpression((VariableReferenceExpression)node, si);
                    break;
                case CodeNodeType.UnaryExpression:
                    EmitUnaryExpression((UnaryExpression)node, si);
                    break;
                default:
                    Helper.NotSupported();
                    break;
            }
        }

        private void EmitArgumentReferenceExpression(ArgumentReferenceExpression node, int si)
        {
            this.Text.Emit("movl {0}(%ebp), %eax", this._parameterLocations[node.Parameter.Name]);
        }

        public void EmitReturnStatement(ReturnStatement node, int si)
        {
            Emit(node.Expression, si);
            this.Text.Emit("leave");
            this.Text.Emit("ret");
        }

        public void EmitLiteralExpression(LiteralExpression node, int si)
        {
            if(node.Value is int)
            {
                this.Text.Emit("movl ${0}, %eax", node.Value);
            }
            else if(node.Value is float)
            {
                var value = (float)node.Value;
                var label = this.ROData.Label(section => section.EmitLong(value.ToIEEE754()));
                this.Text.Emit("flds {0}", label);
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
                    this.Text.Emit("notl %eax");
                    break;
                case UnaryOperator.Negate:
                    this.Text.Emit("negl %eax");
                    break;
                case UnaryOperator.LogicalNot:
                default:
                    Helper.NotSupported();
                    break;
            }
        }

        public void EmitAssignExpression(AssignExpression node, int si)
        {
            EmitExpression(node.Expression, si);
            this.Text.Emit("movl %eax, {0}(%ebp)", LookupVariable(node.Target));
        }

        private int LookupVariable(Expression node)
        {
            var var = node as VariableReferenceExpression;
            if (var == null)
                Helper.NotSupported();
            
            return _variableLocations[var.Variable.Name];
        }

       

        public void EmitBinaryExpression(TypedBinaryExpression node, int si)
        {
            EmitExpression(node.Right, si);
            this.Text.Emit("movl %eax, {0}(%ebp)", si);
            EmitExpression(node.Left, si + 4);

            switch (node.Operator)
            {
                case BinaryOperator.Add:
                    this.Text.Emit("addl {0}(%ebp), %eax", si);
                    break;
                case BinaryOperator.Subtract:
                    this.Text.Emit("subl {0}(%ebp), %eax", si);
                    break;
                case BinaryOperator.BitwiseAnd:
                    this.Text.Emit("andl {0}(%ebp), %eax", si);
                    break;
                case BinaryOperator.BitwiseOr:
                    this.Text.Emit("orl {0}(%ebp), %eax", si);
                    break;
                case BinaryOperator.BitwiseXor:
                    this.Text.Emit("xorl {0}(%ebp), %eax", si);
                    break;
                case BinaryOperator.ValueEquality:
                    EmitComparePattern(string.Format("{0}(%ebp)", si), "%eax",
                        () => this.Text.Emit("movl ${0}, %eax", 1),
                        () => this.Text.Emit("movl ${0}, %eax", 0));
                    break;
                case BinaryOperator.ValueInequality:
                    EmitComparePattern(string.Format("{0}(%ebp)", si), "%eax",
                        () => this.Text.Emit("movl ${0}, %eax", 0),
                        () => this.Text.Emit("movl ${0}, %eax", 1));
                    break;
                case BinaryOperator.LessThan:
                    EmitComparePattern(string.Format("{0}(%ebp)", si), "%eax",
                        () => this.Text.Emit("movl ${0}, %eax", 1),
                        () => this.Text.Emit("movl ${0}, %eax", 0), node.IsSigned() ? "jl" : "jb");
                    break;
                case BinaryOperator.LessThanOrEqual:
                    EmitComparePattern(string.Format("{0}(%ebp)", si), "%eax",
                        () => this.Text.Emit("movl ${0}, %eax", 1),
                        () => this.Text.Emit("movl ${0}, %eax", 0), node.IsSigned() ? "jle" : "jbe");
                    break;
                case BinaryOperator.GreaterThan:
                    EmitComparePattern(string.Format("{0}(%ebp)", si), "%eax",
                        () => this.Text.Emit("movl ${0}, %eax", 1),
                        () => this.Text.Emit("movl ${0}, %eax", 0), node.IsSigned() ? "jg" : "ja");
                    break;
                case BinaryOperator.GreaterThanOrEqual:
                    EmitComparePattern(string.Format("{0}(%ebp)", si), "%eax",
                        () => this.Text.Emit("movl ${0}, %eax", 1),
                        () => this.Text.Emit("movl ${0}, %eax", 0), node.IsSigned() ? "jge" : "jae");
                    break;
                case BinaryOperator.Divide:
                case BinaryOperator.LogicalOr:
                case BinaryOperator.LogicalAnd:
                case BinaryOperator.LeftShift:
                case BinaryOperator.RightShift:
                case BinaryOperator.Modulo:
                case BinaryOperator.Multiply:
                default:
                    Helper.NotSupported();
                    break;
            }
        }

        public void EmitVariableReferenceExpression(VariableReferenceExpression node, int si)
        {
            this.Text.Emit("movl {0}(%ebp), %eax", this._variableLocations[node.Variable.Name]);
        }

      
    }
}


