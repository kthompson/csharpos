using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Framework;

namespace Compiler
{
    public class CilToX86Stage : MethodCompilerStageBase
    {
        public override string Name
        {
            get { return "Cil to x86 Stage"; }
        }

        public override IMethodCompilerContext Run(IMethodCompilerContext context)
        {
            var code = context as CodeStream;
            if (code == null)
                return null;

            foreach (var instruction in code)
            {
                
            }

            return base.Run(context);
        }
    }
}
