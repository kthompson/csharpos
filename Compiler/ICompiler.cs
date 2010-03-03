using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    public interface ICompiler
    {
        List<ICompilerStage> Stages { get; }
        void Compile(ICompilerContext context);
    }
}
