using Mono.Cecil;
using Translator;

namespace Compiler
{
    public interface IAssemblyCompiler : ICompiler
    {
        AssemblyDefinition Assembly { get; }
    }
}


