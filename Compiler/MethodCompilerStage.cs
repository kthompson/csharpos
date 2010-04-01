using System;
using System.Collections.Generic;
using System.IO;
using Compiler.Framework;
using Mono.Cecil;
using Cecil.Decompiler;

namespace Compiler
{
    public class MethodCompilerStage :  CompilerStageBase
    {
        #region IAssemblyCompiler Members
        public override IAssemblyCompilerContext Run(IAssemblyCompiler compiler, IAssemblyCompilerContext context)
        {
            var assemblyContext = context as AssemblyCompilerContext;
            if (assemblyContext == null)
                return context;

            var list = new List<IMethodCompilerContext>(assemblyContext.MethodContexts);
            assemblyContext.MethodContexts.Clear();

            foreach (var mc in list)
                assemblyContext.MethodContexts.Add(compiler.MethodCompiler.Compile(mc));
            
            return assemblyContext;
        }
        #endregion

        public override string Name
        {
            get { return "Method Compiler"; }
        }

    }
}


