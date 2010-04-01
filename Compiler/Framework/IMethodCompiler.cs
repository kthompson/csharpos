using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler.Framework
{
    public interface IMethodCompiler
    {
        IArchitecture Architecture { get; }
        List<IMethodCompilerStage> Stages { get; }
        IMethodCompilerContext GetContext(IAssemblyCompilerContext context, MethodReference method);
        IMethodCompilerContext Compile(IMethodCompilerContext context);
    }
}