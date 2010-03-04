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
            var assemblyContext = context as AssemblyCompilerContext;
            if (assemblyContext == null)
                return context;

            foreach (var method in assemblyContext.Methods)
                EmitMethod(assemblyContext, method);

            return assemblyContext;
        }
        #endregion

        public override string Name
        {
            get { return "Method Compiler"; }
        }

        private void EmitMethod(AssemblyCompilerContext assemblyContext, MethodDefinition method)
        {
            using (var writer = assemblyContext.GetOutputFileWriter(Helper.GetRandomString(method.Name + "-", 40, ".s")))
            {
                var emitter = new Emitter(writer);
                emitter.VisitMethodDefinition(method);
            }
        }
    }
}


