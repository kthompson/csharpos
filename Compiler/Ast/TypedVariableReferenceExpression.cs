using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cecil.Decompiler.Ast;
using Mono.Cecil.Metadata;

namespace Compiler.Ast
{
    public class TypedVariableReferenceExpression : VariableReferenceExpression, ITypedCodeNode
    {
        public TypedVariableReferenceExpression(Mono.Cecil.Cil.VariableReference variable)
            : base(variable)
        {
            var type = Type.GetType(variable.VariableType.FullName, true);

            this.ElementType = TypedTransformer.GetElementType(type);
        }

        #region ITypedCodeNode Members

        public ElementType ElementType
        {
            get;
            private set;
        }

        #endregion
    }
}
