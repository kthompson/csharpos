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
            Visit(node.Expression);
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

        public override void VisitUnaryExpression(UnaryExpression node)
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
            
            base.VisitUnaryExpression(node);
        }

        public override void VisitAddressDereferenceExpression(AddressDereferenceExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitAddressOfExpression(AddressOfExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitAddressReferenceExpression(AddressReferenceExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitArgumentReferenceExpression(ArgumentReferenceExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitArrayCreationExpression(ArrayCreationExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitArrayIndexerExpression(ArrayIndexerExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitAssignExpression(AssignExpression node)
        {
            Visit(node.Expression);
            VisitSaveExpression(node.Target);
        }

        public override void VisitBaseReferenceExpression(BaseReferenceExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitBinaryExpression(BinaryExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitBlockExpression(BlockExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitBreakStatement(BreakStatement node)
        {
            Helper.NotSupported();
        }

        public override void VisitCanCastExpression(CanCastExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitCastExpression(CastExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitCatchClause(CatchClause node)
        {
            Helper.NotSupported();
        }

        public override void VisitConditionCase(ConditionCase node)
        {
            Helper.NotSupported();
        }

        public override void VisitConditionExpression(ConditionExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitContinueStatement(ContinueStatement node)
        {
            Helper.NotSupported();
        }

        public override void VisitDefaultCase(DefaultCase node)
        {
            Helper.NotSupported();
        }

        public override void VisitDelegateCreationExpression(DelegateCreationExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitDelegateInvocationExpression(DelegateInvocationExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitDoWhileStatement(DoWhileStatement node)
        {
            Helper.NotSupported();
        }

        public override void VisitFieldReferenceExpression(FieldReferenceExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitForEachStatement(ForEachStatement node)
        {
            Helper.NotSupported();
        }

        public override void VisitForStatement(ForStatement node)
        {
            Helper.NotSupported();
        }

        public override void VisitGotoStatement(GotoStatement node)
        {
            Helper.NotSupported();
        }

        public override void VisitIfStatement(IfStatement node)
        {
            Helper.NotSupported();
        }

        public override void VisitLabeledStatement(LabeledStatement node)
        {
            Helper.NotSupported();
        }

        public override void VisitMethodInvocationExpression(MethodInvocationExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitMethodReferenceExpression(MethodReferenceExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitNullCoalesceExpression(NullCoalesceExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitObjectCreationExpression(ObjectCreationExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitPropertyReferenceExpression(PropertyReferenceExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitSafeCastExpression(SafeCastExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitSwitchStatement(SwitchStatement node)
        {
            Helper.NotSupported();
        }

        public override void VisitThisReferenceExpression(ThisReferenceExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitThrowStatement(ThrowStatement node)
        {
            Helper.NotSupported();
        }

        public override void VisitTryStatement(TryStatement node)
        {
            Helper.NotSupported();
        }

        public override void VisitTypeOfExpression(TypeOfExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitTypeReferenceExpression(TypeReferenceExpression node)
        {
            Helper.NotSupported();
        }

        public override void VisitVariableDeclarationExpression(VariableDeclarationExpression node)
        {
            Helper.NotSupported();
        }

        public virtual void VisitSaveExpression(Expression node)
        {
            if (node is VariableReferenceExpression)
                VisitSaveVariableReferenceExpression((VariableReferenceExpression)node);
            else
                Helper.NotSupported();
        }

        public override void VisitVariableReferenceExpression(VariableReferenceExpression node)
        {
            //move eax into variable
            this.Text.Emit(X86.OpCodes.Move.Create(this._variableLocations[node.Variable.Name] + "(%esp)", "%eax"));
        }

        public virtual void VisitSaveVariableReferenceExpression(VariableReferenceExpression node)
        {
            //move variable into eax
            this.Text.Emit(X86.OpCodes.Move.Create("%eax", this._variableLocations[node.Variable.Name] + "(%esp)"));
        }

        public override void VisitWhileStatement(WhileStatement node)
        {
            Helper.NotSupported();
        }
    }
}


