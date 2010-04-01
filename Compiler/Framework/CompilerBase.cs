using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Framework
{
    public abstract class CompilerBase : IAssemblyCompiler
    {
        public IArchitecture Architecture { get; private set; }
        public List<IAssemblyCompilerStage> Stages { get; private set; }
        public IMethodCompiler MethodCompiler { get; private set; }

        protected CompilerBase(IArchitecture arch, IMethodCompiler methodCompiler, IEnumerable<IAssemblyCompilerStage> stages)
        {
            this.Architecture = arch;
            this.MethodCompiler = methodCompiler;
            this.Stages = new List<IAssemblyCompilerStage>(stages);
        }

        protected virtual void OnBeforeCompile(IAssemblyCompilerContext context)
        {
        }

        protected virtual void OnAfterCompile(IAssemblyCompilerContext context)
        {
        }

        public IAssemblyCompilerContext Compile(IAssemblyCompilerContext context)
        {
            OnBeforeCompile(context);
            context = this.Stages.Aggregate(context, (current, stage) => stage.Run(this, current));
            OnAfterCompile(context);
            return context;
        }
    }
}