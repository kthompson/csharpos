using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cecil.Decompiler.Ast;

namespace Compiler.Ast
{
    public class TypedAssignExpression : AssignExpression, ITypedCodeNode
    {
        public TypedAssignExpression(Expression target, Expression expression)
            : base(target, expression)
        {
            this.ElementType = TypedTransformer.GetElementType(target);
            //TODO: do we need to check the assigning type? if so how can we do it without the actual type handles?
            //Helper.AreEqual(this.ElementType, TypedTransformer.GetElementType(expression), "'expression' argument must have the same element type as 'target' argument.");
        }

        #region ITypedCodeNode Members

        public Mono.Cecil.Metadata.ElementType ElementType
        {
            get;
            private set;
        }

        #endregion
    }
}
