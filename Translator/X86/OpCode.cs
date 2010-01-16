using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;

namespace Compiler.X86
{
    public class OpCode : IOpCode
    {
        public Code Code { get; private set; }
        public string Format { get; private set; }

        internal OpCode(Code code, string format)
        {
            this.Format = format;
            this.Code = code;
        }

        #region IOpCode Members

        byte IOpCode.Code
        {
            get
            {
                return (byte)this.Code;
            }
        }

        #endregion
    }
}
