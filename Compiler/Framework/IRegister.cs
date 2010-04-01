using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler.Framework
{
    public interface IRegister
    {
        bool CanStore(TypeReference type);

        string Name { get; }
        int Size { get; }

        IRegister High { get; }
        IRegister Low { get; }
    }
}
