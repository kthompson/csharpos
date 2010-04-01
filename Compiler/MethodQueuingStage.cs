using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Framework;
using Mono.Cecil;

namespace Compiler
{
    /// <summary>
    /// Stage that will queue methods for compilation
    /// </summary>
    public class MethodQueuingStage : CompilerStageBase
    {
        public override string Name
        {
            get { return "MethodQueuingStage"; }
        }

        public override IAssemblyCompilerContext Run(IAssemblyCompiler compiler, IAssemblyCompilerContext context)
        {
            var ac = context as AssemblyCompilerContext;
            Helper.IsNotNull(ac);

            var asm = ac.AssemblyDefinition;
            var mc = compiler.MethodCompiler;
            foreach (ModuleDefinition module in asm.Modules)
                foreach (TypeDefinition type in module.Types)
                    foreach (MethodDefinition method in type.Methods)
                        ac.MethodContexts.Add(mc.GetContext(context, method));

            return ac;
        }
    }
}
