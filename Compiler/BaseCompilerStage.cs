using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    public abstract class BaseCompilerStage : ICompilerStage
    {
        public abstract string Name { get; }

        public virtual ICompilerContext Run(ICompilerContext context)
        {
            return context;
        }
    }
}
