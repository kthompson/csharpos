using System.Linq;
using Mono.Cecil.Cil;

namespace Compiler.X86
{
    public class X86Instruction : IInstruction<OpCode>
    {
        public X86Instruction(OpCode opcode, params object[] operands)
        {
            this.OpCode = opcode;
            this.Operands = operands;
        }

        public OpCode OpCode { get; private set; }
        public object[] Operands { get; private set; }
        public IInstruction Next { get; set; }
        public IInstruction Previous { get; set; }

        public override string ToString()
        {
            return string.Format(this.OpCode.Format, this.Operands);
        }

        #region IInstruction Members

        public int Offset { get; set; }

        IOpCode IInstruction.OpCode
        {
            get { return this.OpCode; }
        }

        public int GetSize()
        {
            return 0;
        }

        #endregion

        #region IInstruction Members


        public object Operand
        {
            get { return this.Operands.FirstOrDefault(); }
        }

        #endregion
    }
}


