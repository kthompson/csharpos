using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Translator
{
    public interface IMethodCompiler : ICompiler
    {
        MethodReference Method { get; }
        IEmitter Emitter { get; }
    }
}
