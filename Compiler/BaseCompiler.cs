using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    public abstract class BaseCompiler : ICompiler
    {
        public List<ICompilerStage> Stages { get; private set; }

        protected BaseCompiler(params ICompilerStage[] stages)
        {
            this.Stages = new List<ICompilerStage>(stages);
        }

        protected virtual void OnBeforeCompile(ICompilerContext context)
        {
        }

        protected virtual void OnAfterCompile(ICompilerContext context)
        {
        }

        public void Compile(ICompilerContext context)
        {
            OnBeforeCompile(context);
            context = this.Stages.Aggregate(context, (current, stage) => stage.Run(current));
            OnAfterCompile(context);
        }
    }
}
