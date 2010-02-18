using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.X86
{
    public static class OpCodes
    {
        public static readonly OpCode Move = new OpCode(Code.Move, "movl {0}, {1}");
        public static readonly OpCode Return = new OpCode(Code.Return, "ret");

        public static readonly OpCode LoadReal = new OpCode(Code.LoadReal, "flds {0}");
    }
}
