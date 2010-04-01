using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Framework
{
    public interface IArchitecture
    {
        string Name { get; }
        IRegister[] Registers { get; }
        IRegister StackRegister { get; }
    }
}
