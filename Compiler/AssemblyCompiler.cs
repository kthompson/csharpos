using System;
using Mono.Cecil;

namespace Compiler
{
    public class AssemblyCompiler : BaseCompiler
    {
        public AssemblyCompiler(params ICompilerStage[] stages)
            : base(stages)
        {
        }

        protected override void OnAfterCompile(ICompilerContext context)
        {
            //save resultant context info
        }
    }
}
