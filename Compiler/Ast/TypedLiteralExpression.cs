using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cecil.Decompiler.Ast;
using Mono.Cecil.Metadata;

namespace Compiler.Ast
{
    public class TypedLiteralExpression : LiteralExpression, ITypedCodeNode
    {
        public TypedLiteralExpression(object value)
            : base(value)
        {
            this.ElementType = TypedTransformer.GetElementType(value);
        }

        #region ITypedCodeNode Members

        public ElementType ElementType
        {
            get; private set;
        }

        #endregion
    }
}
