using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Indy.IL2CPU.Assembler;
using System.Reflection;
using Mono.Cecil;

namespace Indy.IL2CPU.IL
{
    public abstract class Op
    {
        private readonly string mCurrentInstructionLabel;
        private readonly string mILComment;
        public static string GetInstructionLabel(Mono.Cecil.Cil.Instruction instruction)
        {
            return GetInstructionLabel(instruction.Offset);
        }
        public static string GetInstructionLabel(long aPosition)
        {
            return ".L" + aPosition.ToString("X8");
        }

        public delegate void QueueMethodHandler(MethodDefinition aMethod);

        public delegate void QueueStaticFieldHandler(FieldDefinition aField);

        public void Assemble()
        {
            if (!String.IsNullOrEmpty(mCurrentInstructionLabel))
            {
                new Label(mCurrentInstructionLabel);
            }
            if (!String.IsNullOrEmpty(mILComment))
            {
                new Comment(mILComment);
            }
            AssembleHeader();
            DoAssemble();
        }

        protected virtual void AssembleHeader()
        {
        }

        public abstract void DoAssemble();

        public Op(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
        {
            if (instruction != null)
            {
                mCurrentInstructionLabel = GetInstructionLabel(instruction);
                // todo: need to add the real operand here?
                mILComment = "; IL: " + instruction.OpCode + " " + instruction.Operand.ToString();
            }
        }

        // This is a prop and not a constructor arg for two reasons. Ok, mostly for one
        // 1 - Adding a parameterized triggers annoying C# constructor hell, every descendant we'd have to reimplement it
        // 2 - This helps to allow changing of assembler while its in use. Currently no idea why we would ever want to do that
        // rather than construct a new one though....
        // If we end up with more things we need, probably will change to Initialize(x, y), or someone can go thorugh and add
        // all the friggin constructors
        protected Assembler.Assembler mAssembler;
        public Assembler.Assembler Assembler
        {
            get
            {
                return mAssembler;
            }
            set
            {
                mAssembler = value;
            }
        }

        protected void OnQueueMethod(MethodDefinition aMethod)
        {
            var handler = QueueMethod;
            if (handler != null)
                QueueMethod(aMethod);
        }

        protected void OnQueueStaticField(FieldDefinition aField)
        {
            var handler = QueueStaticField;
            if (handler != null && aField.IsStatic)
                QueueStaticField(aField);
        }

        public static event QueueMethodHandler QueueMethod;
        public static event QueueStaticFieldHandler QueueStaticField;
    }
}