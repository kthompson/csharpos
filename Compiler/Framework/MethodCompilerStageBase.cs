using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Framework
{
    public abstract class MethodCompilerStageBase : IMethodCompilerStage
    {
        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }


        public virtual IMethodCompilerContext Run(IMethodCompilerContext context)
        {
            return context;
        }
    }
}
