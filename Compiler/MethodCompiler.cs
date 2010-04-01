using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Framework;

namespace Compiler
{
    public class MethodCompiler : MethodCompilerBase
    {
        public MethodCompiler(IArchitecture arch, params IMethodCompilerStage[] stages)
            : base(arch, stages)
        {

        }
    }
}
