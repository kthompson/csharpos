using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    public interface IInstruction
    {
        IInstruction Next { get; set; }
        IInstruction Previous { get; set; }

        bool IsCIL { get; }
        object[] Operands { get; set; }
    }
}
