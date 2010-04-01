using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Framework
{
    public class Label
    {
        public string Name { get; private set; }

        public Label(string name)
        {
            Helper.IsNotNullOrEmpty(name);
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
