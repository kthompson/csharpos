using System;
using System.Collections.Generic;
using CPU = Indy.IL2CPU.Assembler;
using CPUx86 = Indy.IL2CPU.Assembler.X86;
using System.Reflection;
using Indy.IL2CPU.Assembler;
using Mono.Cecil;

namespace Indy.IL2CPU.IL.X86
{
    [OpCode(Mono.Cecil.Cil.Code.Newarr)]
    public class Newarr : Op
    {
        private uint mElementSize;
        private string mCtorName;
        public static void ScanOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo, SortedList<string, object> aMethodData)
        {
            var typeRef = instruction.Operand as TypeReference;
            if (typeRef == null)
            {
                throw new Exception("No TypeRef found!");
            }
            Engine.RegisterType(typeRef);
            var arrayType = Engine.GetType("mscorlib", "System.Array");
            Engine.RegisterType(arrayType);
            var ctor = TypeResolver.GetConstructor(arrayType);
            Engine.QueueMethod(ctor);
        }

        public Newarr(TypeReference typeRef, string aBaseLabelName)
            : base(null, null)
        {
            Initialize(typeRef, aBaseLabelName);
        }

        public Newarr(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            var typeRef = instruction.Operand as TypeReference;
            if (typeRef == null)
            {
                throw new Exception("No TypeRef found!");
            }
            Initialize(typeRef, GetInstructionLabel(instruction));
        }

        private void Initialize(TypeReference aTypeRef, string aBaseLabelName)
        {
            mElementSize = Engine.GetFieldStorageSize(aTypeRef);
            var arrayType = Engine.GetType("mscorlib", "System.Array");
            var constructor = TypeResolver.GetConstructor(arrayType);
            mCtorName = CPU.Label.GenerateLabelName(constructor);
            Engine.QueueMethod(constructor);
            OnQueueMethod(GCImplementationRefs.AllocNewObjectRef);
        }

        public override void DoAssemble()
        {
            new CPU.Comment("Element Size = " + mElementSize);
            // element count is on the stack
            int xElementCountSize = Assembler.StackContents.Pop().Size;
            new CPUx86.Pop { DestinationReg = CPUx86.Registers.ESI };
            new CPUx86.Push { DestinationReg = CPUx86.Registers.ESI };
            //Assembler.StackSizes.Push(xElementCountSize);
            new CPUx86.Push { DestinationValue = mElementSize };
            Assembler.StackContents.Push(new StackContent(4, typeof(uint)));
            Multiply(Assembler);
            // the total items size is now on the stack
            new CPUx86.Push { DestinationValue = (ObjectImpl.FieldDataOffset + 4) };
            Assembler.StackContents.Push(new StackContent(4, typeof(uint)));
            Add(Assembler);
            // the total array size is now on the stack.
            Engine.QueueMethod(GCImplementationRefs.AllocNewObjectRef);
            new CPUx86.Call { DestinationLabel = CPU.Label.GenerateLabelName(GCImplementationRefs.AllocNewObjectRef) };
            new CPUx86.Push { DestinationReg = CPUx86.Registers.ESP, DestinationIsIndirect = true };
            new CPUx86.Push { DestinationReg = CPUx86.Registers.ESP, DestinationIsIndirect = true };
            new CPUx86.Push { DestinationReg = CPUx86.Registers.ESP, DestinationIsIndirect = true };
            new CPUx86.Push { DestinationReg = CPUx86.Registers.ESP, DestinationIsIndirect = true };
            //new CPUx86.Pushd(CPUx86.Registers_Old.EDI);
            Engine.QueueMethod(GCImplementationRefs.IncRefCountRef);
            new CPUx86.Call { DestinationLabel = CPU.Label.GenerateLabelName(GCImplementationRefs.IncRefCountRef) };
            new CPUx86.Call { DestinationLabel = CPU.Label.GenerateLabelName(GCImplementationRefs.IncRefCountRef) };
            //new CPUx86.Pop(CPUx86.Registers_Old.ESI);
            Assembler.StackContents.Push(new StackContent(4, typeof(Array)));
            new CPUx86.Pop { DestinationReg = CPUx86.Registers.EAX };
            new CPUx86.Move { DestinationReg = CPUx86.Registers.EAX, DestinationIsIndirect = true, SourceValue = (uint)Engine.RegisterType(Engine.GetType("mscorlib", "System.Array")), Size = 32 };
            new CPUx86.Add { DestinationReg = CPUx86.Registers.EAX, SourceValue = 4 };
            new CPUx86.Move { DestinationReg = CPUx86.Registers.EAX, DestinationIsIndirect = true, SourceValue = (uint)InstanceTypeEnum.Array, Size = 32 };
            new CPUx86.Add { DestinationReg = CPUx86.Registers.EAX, SourceValue = 4 };
            new CPUx86.Move { DestinationReg = CPUx86.Registers.EAX, DestinationIsIndirect = true, SourceReg = CPUx86.Registers.ESI, Size = 32 };
            new CPUx86.Add { DestinationReg = CPUx86.Registers.EAX, SourceValue = 4 };
            new CPUx86.Move { DestinationReg = CPUx86.Registers.EAX, DestinationIsIndirect = true, SourceValue = (uint)mElementSize, Size = 32 };
            new CPUx86.Call { DestinationLabel = mCtorName };
        }
    }
}