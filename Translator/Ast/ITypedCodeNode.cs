using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cecil.Decompiler.Ast;
using Mono.Cecil.Metadata;

namespace Compiler.Ast
{
    public interface ITypedCodeNode : ICodeNode
    {
        ElementType ElementType { get; }
    }
}
