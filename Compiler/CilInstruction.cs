using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;

namespace Compiler
{
    public class CilInstruction : IInstruction
    {
        public int Offset { get; private set; }
        public OpCode OpCode { get; private set; }
        public object[] Operands { get; set; }

        public CilInstruction(Instruction instruction)
        {
            this.Offset = instruction.Offset;
            this.OpCode = instruction.OpCode;
            this.Operands = new[] {instruction.Operand};
        }

        #region IInstruction Members

        public IInstruction Next
        {
            get; set;
        }

        public IInstruction Previous
        {
            get; set;
        }

        #endregion

        #region IInstruction Members


        public bool IsCIL
        {
            get { return true; }
        }

        #endregion
    }
}
