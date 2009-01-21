using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Translator
{
    public interface ITranslator
    {
        IMethod TranslateMethod(MethodDefinition method);
        List<MethodReference> CollectMethodReferences(MethodDefinition method);
    }
}
