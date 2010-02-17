using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cecil.Decompiler.Ast;

namespace Compiler.Ast
{
    public class TypedBinaryExpression : BinaryExpression, ITypedCodeNode
    {
        public TypedBinaryExpression(BinaryOperator @operator, Expression left, Expression right)
            : base(@operator, left, right)
        {
            this.ElementType = TypedTransformer.GetElementType(left);
            Helper.AreEqual(this.ElementType, TypedTransformer.GetElementType(right), "left and right expressions do not have the same ElementType");
        }

        #region ITypedCodeNode Members

        public Mono.Cecil.Metadata.ElementType ElementType
        {
            get; private set;
        }

        #endregion
    }
}
