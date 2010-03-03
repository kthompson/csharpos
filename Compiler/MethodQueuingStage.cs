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
        public List<MethodCompilerContext> Methods { get; private set; }

        public override string Name
        {
            get { return "MethodQueuingStage"; }
        }

        public MethodQueuingStage()
        {
            this.Methods = new List<MethodCompilerContext>();
        }

        public override ICompilerContext Run(ICompilerContext context)
        {
            if ((context is AssemblyCompilerContext))
            {
                var asm = ((AssemblyCompilerContext) context).AssemblyDefinition;
                this.Methods.Add(new MethodCompilerContext(asm.EntryPoint));
            }

            return context;
        }
    }
}
