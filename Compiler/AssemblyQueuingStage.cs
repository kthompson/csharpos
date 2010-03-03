using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler
{
    /// <summary>
    /// Compiler stage that will queue any dependencies based on an AssemblyCompilerContext
    /// </summary>
    public class AssemblyQueuingStage : BaseCompilerStage
    {
        public override string Name
        {
            get { return "Assembly Queuing Stage"; }
        }

        public override ICompilerContext Run(ICompilerContext context)
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
