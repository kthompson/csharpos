using System;
using System.IO;
using Mono.Cecil;
using Cecil.Decompiler;

namespace Compiler
{
    public class MethodCompilerStage :  BaseCompilerStage
    {
        #region ICompiler Members
        public override ICompilerContext Run(ICompilerContext context)
        {
            var methodContext = context as MethodCompilerContext;
            if (methodContext == null)
                return context;
            
            var emitter = new Emitter(methodContext.Out);
            emitter.VisitMethodDefinition(methodContext.Method.Resolve());

            return methodContext;
        }

        #endregion

        public override string Name
        {
            get { return "Method Compiler"; }
        }
    }
}


