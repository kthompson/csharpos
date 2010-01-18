using Mono.Cecil;

namespace Compiler
{
    public interface IAssemblyCompiler : ICompiler
    {
        AssemblyDefinition Assembly { get; }
    }
}


