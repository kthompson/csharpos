using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cecil.Decompiler.Ast;
using Mono.Cecil;

namespace Compiler.Ast
{
    public class TypedArgumentReferenceExpression : ArgumentReferenceExpression, ITypedCodeNode
    {
        public TypedArgumentReferenceExpression(ParameterReference parameter)
            : base(parameter)
        {
            this.ElementType = TypedTransformer.GetElementType(parameter.ParameterType);
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
