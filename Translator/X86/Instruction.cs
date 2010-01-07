using System;
using System.Collections.Generic;
using System.Text;

namespace Translator.X86
{
    public class Instruction
    {
        public string Mneumonic { get; private set; }
        public Operand[] Operands { get; private set; }
    }
}
