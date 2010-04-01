using System;
using System.Collections.Generic;
using Compiler.Framework;
using Mono.Cecil;

namespace Compiler
{
    public class AssemblyCompiler : CompilerBase
    {
        public AssemblyCompiler(IArchitecture architecture, IMethodCompiler mc, IEnumerable<IAssemblyCompilerStage> stages)
            : base(architecture, mc, stages)
        {
        }

        protected override void OnAfterCompile(IAssemblyCompilerContext context)
        {
            //save resultant context info
        }
    }
}
