using Mono.Cecil;

namespace Compiler
{
    public interface IMethodCompiler : ICompiler
    {
        MethodReference Method { get; }
        IEmitter Emitter { get; }
    }
}


