using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Framework;

namespace Compiler.x86
{
    public class Architecture : ArchitectureBase
    {
        public override IRegister[] Registers
        {
            get { return X86.Registers.All; }
        }

        public override IRegister StackRegister
        {
            get { return X86.Registers.Esp; }
        }

        public override string Name
        {
            get { return "x86"; }
        }
    }
}
