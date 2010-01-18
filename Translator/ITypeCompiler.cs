using Mono.Cecil;

namespace Compiler
{
    public interface ITypeCompiler : ICompiler
    {
        TypeReference Type { get; }
    }
}


