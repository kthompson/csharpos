using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Framework
{
    public abstract class CompilerStageBase : IAssemblyCompilerStage
    {
        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public virtual IAssemblyCompilerContext Run(IAssemblyCompiler compiler, IAssemblyCompilerContext context)
        {
            return context;
        }
    }
}