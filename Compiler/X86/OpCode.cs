using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cecil.Decompiler.Ast;
using Mono.Cecil.Cil;

namespace Compiler.X86
{
    public class OpCode 
    {
        public Code Code { get; private set; }
        public string Format { get; private set; }

        internal OpCode(Code code, string format)
        {
            this.Format = format;
            this.Code = code;
        }

        public string Create(params object[] operands)
        {
            return string.Format(this.Format, operands);
        }
    }
}
