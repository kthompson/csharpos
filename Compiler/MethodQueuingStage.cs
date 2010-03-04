using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler
{
    /// <summary>
    /// Stage that will queue methods for compilation
    /// </summary>
    public class MethodQueuingStage : BaseCompilerStage
    {
        public override string Name
        {
            get { return "MethodQueuingStage"; }
        }

        public MethodQueuingStage()
        {
        }

        public override ICompilerContext Run(ICompilerContext context)
        {
            var assemblyContext = context as AssemblyCompilerContext;
            if (assemblyContext == null)
                return context;

            var asm = assemblyContext.AssemblyDefinition;
            assemblyContext.Methods.Add(asm.EntryPoint);

            return assemblyContext;
        }
    }
}
