using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Framework;
using Mono.Cecil;

namespace Compiler
{
    /// <summary>
    /// Compiler stage that will queue any dependencies based on an AssemblyCompilerContext
    /// </summary>
    public class AssemblyQueuingStage : CompilerStageBase
    {
        public override string Name
        {
            get { return "Assembly Queuing Stage"; }
        }

        public override IAssemblyCompilerContext Run(IAssemblyCompiler compiler, IAssemblyCompilerContext context)
        {
            var asmcc = context as AssemblyCompilerContext;
            if (asmcc == null)
                return context;

            var asm = asmcc.AssemblyDefinition;
            foreach (AssemblyNameReference asmRef in asm.MainModule.AssemblyReferences)
            {
                
            }

            return context;
        }
    }
}
