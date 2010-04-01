using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler.Framework
{
    public interface IMethodCompilerContext
    {
        IAssemblyCompilerContext AssemblyCompilerContext { get; }
        MethodDefinition Method { get; }
        Label GetUniqueLabel();
    }
}