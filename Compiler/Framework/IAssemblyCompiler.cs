using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Framework
{
    public interface IAssemblyCompiler
    {
        IArchitecture Architecture { get; }
        IMethodCompiler MethodCompiler { get; }
        List<IAssemblyCompilerStage> Stages { get; }
        IAssemblyCompilerContext Compile(IAssemblyCompilerContext context);
    }
}