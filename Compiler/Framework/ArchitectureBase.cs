using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Framework
{
    public abstract class ArchitectureBase : IArchitecture
    {
        #region IArchitecture Members

        public abstract IRegister[] Registers { get; }
        public abstract IRegister StackRegister { get; }
        public abstract string Name { get; }

        #endregion
    }
}
