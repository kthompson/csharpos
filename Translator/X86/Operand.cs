namespace Compiler.X86
{
    public class Operand
    {
        public AddressingMethod AddressingMethod { get; private set; }
        public OperandType Type { get; private set; }

        public Operand(AddressingMethod addressingMethod, OperandType type)
        {
            this.AddressingMethod = addressingMethod;
            this.Type = type;
        }
    }
}


