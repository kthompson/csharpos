using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.X86
{
    public class Register
    {
        public string Name { get; private set; }
        public int Size { get; private set; }

        internal Register(string name, int size)
        {
            this.Name = name;
            this.Size = size;
        }

        public override string ToString()
        {
            return string.Format("%{0}", this.Name);
        }
    }
}
