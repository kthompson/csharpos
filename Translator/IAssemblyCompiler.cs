using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Translator
{
    public interface IAssemblyCompiler : ICompiler
    {
        AssemblyDefinition Assembly { get; }
    }
}
