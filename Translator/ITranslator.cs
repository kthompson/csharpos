using System.Collections.Generic;
using Mono.Cecil;

namespace Compiler
{
    public interface ITranslator
    {
        List<MethodReference> CollectMethodReferences(MethodDefinition method);
    }
}


