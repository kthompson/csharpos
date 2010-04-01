using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler.Framework
{
    public abstract class MethodCompilerBase : IMethodCompiler
    {
        protected MethodCompilerBase(IArchitecture arch, params IMethodCompilerStage[] stages)
        {
            this.Architecture = arch;
            this.Stages = new List<IMethodCompilerStage>(stages);
        }

        #region IMethodCompiler Members

        public IArchitecture Architecture { get; private set; }
        public List<IMethodCompilerStage> Stages { get; private set; }

        public IMethodCompilerContext Compile(IMethodCompilerContext context)
        {
            OnBeforeCompile(context);
            context = this.Stages.Aggregate(context, (current, stage) => stage.Run(current));
            OnAfterCompile(context);

            return context;
        }

        public IMethodCompilerContext GetContext(IAssemblyCompilerContext context, MethodReference method)
        {
            return CodeStream.Create(context, method);
        }
       
        #endregion

        protected virtual void OnBeforeCompile(IMethodCompilerContext context)
        {
        }

        protected virtual void OnAfterCompile(IMethodCompilerContext context)
        {
        }
    }
}
