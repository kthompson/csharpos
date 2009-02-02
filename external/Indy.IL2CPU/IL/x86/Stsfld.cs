using System;
using System.Collections.Generic;
using Indy.IL2CPU.Assembler;


using CPUx86 = Indy.IL2CPU.Assembler.X86;
using System.Reflection;
using Mono.Cecil;

namespace Indy.IL2CPU.IL.X86
{
    [OpCode(Mono.Cecil.Cil.Code.Stsfld)]
    public class Stsfld : Op
    {
        private string mDataName;
        private uint mSize;
        private TypeReference _dataType;
        private bool mNeedsGC;
        private string mBaseLabel;

        public static void ScanOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo, SortedList<string, object> aMethodData)
        {
            var xField = instruction.Operand as FieldDefinition;
            Engine.QueueStaticField(xField);
        }

        public Stsfld(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            var field = instruction.Operand as FieldReference;
            mSize = Engine.GetFieldStorageSize(field.FieldType);
            Engine.QueueStaticField(field, out mDataName);
            mNeedsGC = !field.FieldType.IsValueType;
            _dataType = field.FieldType;
            mBaseLabel = GetInstructionLabel(instruction);
        }

        public override void DoAssemble()
        {
            if (mNeedsGC)
            {
                new CPUx86.Push { DestinationRef = new ElementReference(mDataName), DestinationIsIndirect = true };
                Engine.QueueMethod(GCImplementationRefs.DecRefCountRef);
                new CPUx86.Call { DestinationLabel = Label.GenerateLabelName(GCImplementationRefs.DecRefCountRef) };
            }
            for (int i = 0; i < (mSize / 4); i++)
            {
                new CPUx86.Pop { DestinationReg = CPUx86.Registers.EAX };
                new CPUx86.Move { DestinationRef = new ElementReference(mDataName, i * 4), DestinationIsIndirect = true, SourceReg = CPUx86.Registers.EAX };
            }
            switch (mSize % 4)
            {
                case 1:
                    {
                        new CPUx86.Pop { DestinationReg = CPUx86.Registers.EAX };
                        new CPUx86.Move { DestinationRef = new ElementReference(mDataName, (int)((mSize / 4) * 4)), DestinationIsIndirect = true, SourceReg = CPUx86.Registers.AL };
                        break;
                    }
                case 2:
                    {
                        new CPUx86.Pop { DestinationReg = CPUx86.Registers.EAX };
                        new CPUx86.Move { DestinationRef = new ElementReference(mDataName, (int)((mSize / 4) * 4)), DestinationIsIndirect = true, SourceReg = CPUx86.Registers.AX };
                        break;
                    }
                case 0:
                    {
                        break;
                    }
                default:
                    throw new Exception("Remainder size " + (mSize % 4) + " not supported!");

            }
            Assembler.StackContents.Pop();
        }
    }
}