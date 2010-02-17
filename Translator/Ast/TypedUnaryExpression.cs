using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cecil.Decompiler.Ast;
using Mono.Cecil.Metadata;

namespace Compiler.Ast
{
    public class TypedUnaryExpression : UnaryExpression, ITypedCodeNode
    {
        public TypedUnaryExpression(UnaryOperator @operator, Expression operand)
            : base(@operator, operand)
        {
            this.ElementType = TypedTransformer.GetElementType(operand);
        }

        #region ITypedCodeNode Members

        public ElementType ElementType
        {
            get; private set;
        }

        #endregion
    }
}
