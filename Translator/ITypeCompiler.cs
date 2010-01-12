using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Translator
{
    public interface ITypeCompiler : ICompiler
    {
        TypeReference Type { get; }
    }
}
