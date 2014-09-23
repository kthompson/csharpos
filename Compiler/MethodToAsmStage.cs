using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Framework;
using Mono.Cecil;

namespace Compiler
{
    public class MethodToAsmStage : MethodCompilerStageBase
    {
        public override IMethodCompilerContext Run(IMethodCompilerContext context)
        {
            context = base.Run(context);
            var acc = context.AssemblyCompilerContext as AssemblyCompilerContext;
            if (acc == null) 
                return context;

            using (var writer = acc.GetOutputFileWriter(GetFilename(context)))
            {
                new Emitter(writer).VisitMethodDefinition(context.Method);
            }

            return context;
        }

        private static string GetFilename(IMethodCompilerContext context)
        {
            return Helper.GetRandomString(context.Method.Name + "-", 40, ".s");
        }
    }
}
