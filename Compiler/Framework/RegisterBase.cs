using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler.Framework
{
    public abstract class RegisterBase : IRegister
    {
        protected RegisterBase(string name, int size)
            : this(name, size, null, null)
        {

        }

        protected RegisterBase(string name, int size, IRegister high, IRegister low)
        {
            this.Name = name;
            this.Size = size;
            this.High = high;
            this.Low = low;
        }

        #region IRegister Members

        public abstract bool CanStore(TypeReference type);

        public string Name { get; private set; }
        public int Size { get; private set; }
        public IRegister High { get; private set; }
        public IRegister Low { get; private set; }

        #endregion
    }
}
