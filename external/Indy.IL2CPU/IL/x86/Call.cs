using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CPU = Indy.IL2CPU.Assembler;
using CPUx86 = Indy.IL2CPU.Assembler.X86;
using System.Reflection;
using Indy.IL2CPU.Assembler;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU.IL.X86
{
    [OpCode(Mono.Cecil.Cil.Code.Call)]
    public class Call : Op
    {
        private string LabelName;
        private uint mResultSize;
        private uint? TotalArgumentSize = null;
        private bool mIsDebugger_Break = false;
        private uint[] ArgumentSizes = new uint[0];
        private MethodInformation mMethodInfo;
        private MethodInformation mTargetMethodInfo;
        private string mNextLabelName;
        private int mCurrentILOffset;

        public static void ScanOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo, SortedList<string, object> aMethodData)
        {
            var method = instruction.Operand as MethodReference;
            ScanOp(method);
        }

        public static void ScanOp(MethodReference targetMethod)
        {

            Engine.QueueMethod(targetMethod);
            foreach (ParameterDefinition param in targetMethod.Parameters)
            {
                Engine.RegisterType(param.ParameterType);
            }
            var xTargetMethodInfo = Engine.GetMethodInfo(targetMethod, targetMethod, Label.GenerateLabelName(targetMethod), Engine.GetTypeInfo(targetMethod.DeclaringType), false);
            Engine.RegisterType(xTargetMethodInfo.ReturnType);
        }

        public Call(MethodDefinition aMethod, uint aCurrentILOffset, bool aDebugMode, uint aExtraStackSpace, string aNormalNext)
            : base(null, null)
        {
            if (aMethod == null)
            {
                throw new ArgumentNullException("aMethod");
            }
            Initialize(aMethod, aCurrentILOffset, aDebugMode);
            mNextLabelName = aNormalNext;
        }

        public Call(MethodDefinition aMethod, uint aCurrentILOffset, bool aDebugMode, string aNormalNext)
            : this(aMethod, aCurrentILOffset, aDebugMode, 0, aNormalNext)
        {
        }

        public static void EmitExceptionLogic(Assembler.Assembler aAssembler, int aCurrentOpOffset, MethodInformation aMethodInfo, string aNextLabel, bool aDoTest, Action aCleanup)
        {
            string xJumpTo = MethodFooterOp.EndOfMethodLabelNameException;
            if (aMethodInfo != null && aMethodInfo.CurrentHandler != null)
            {
                // todo add support for nested handlers, see comment in Engine.cs
                //if (!((aMethodInfo.CurrentHandler.HandlerOffset < aCurrentOpOffset) || (aMethodInfo.CurrentHandler.HandlerLength + aMethodInfo.CurrentHandler.HandlerOffset) <= aCurrentOpOffset)) {
                new CPU.Comment(String.Format("CurrentOffset = {0}, HandlerStartOffset = {1}", aCurrentOpOffset, aMethodInfo.CurrentHandler.HandlerStart.Offset));
                if (aMethodInfo.CurrentHandler.HandlerStart.Offset > aCurrentOpOffset)
                {
                    switch (aMethodInfo.CurrentHandler.Type)
                    {
                        case ExceptionHandlerType.Catch:
                            xJumpTo = Op.GetInstructionLabel(aMethodInfo.CurrentHandler.HandlerStart.Offset);
                            break;
                        case ExceptionHandlerType.Finally:
                            xJumpTo = Op.GetInstructionLabel(aMethodInfo.CurrentHandler.HandlerStart.Offset);
                            break;
                        default:
                            throw new Exception("ExceptionHandlerType '" + aMethodInfo.CurrentHandler.Type.ToString() + "' not supported yet!");
                    }
                }
            }
            if (!aDoTest)
            {
                //new CPUx86.Call("_CODE_REQUESTED_BREAK_");
                new CPUx86.Jump { DestinationLabel = xJumpTo };
            }
            else
            {
                new CPUx86.Test { DestinationReg = CPUx86.Registers.ECX, SourceValue = 2 };
                if (aCleanup != null)
                {
                    new CPUx86.ConditionalJump { Condition = CPUx86.ConditionalTestEnum.Equal, DestinationLabel = aNextLabel };
                    aCleanup();
                    new CPUx86.Jump { DestinationLabel = xJumpTo };
                }
                else
                {
                    new CPUx86.ConditionalJump { Condition = CPUx86.ConditionalTestEnum.NotEqual, DestinationLabel = xJumpTo };
                }
            }
        }

        private void Initialize(MethodReference method, uint aCurrentILOffset, bool aDebugMode)
        {
            Initialize(method.Resolve(), aCurrentILOffset, aDebugMode);
        }

        private void Initialize(MethodDefinition method, uint aCurrentILOffset, bool aDebugMode)
        {
            mIsDebugger_Break = method.ToString() == "System.Void System.Diagnostics.Debugger.Break()";
            if (mIsDebugger_Break)
            {
                return;
            }
            mCurrentILOffset = aCurrentILOffset;
            mTargetMethodInfo = Engine.GetMethodInfo(method, method, Label.GenerateLabelName(method), Engine.GetTypeInfo(method.DeclaringType), aDebugMode);
            mResultSize = 0;
            if (mTargetMethodInfo != null)
            {
                mResultSize = mTargetMethodInfo.ReturnSize;
            }
            LabelName = CPU.Label.GenerateLabelName(method);
            Engine.QueueMethod(method);
            bool needsCleanup = false;
            List<uint> xArgumentSizes = new List<uint>();
            ParameterDefinitionCollection xParams = method.Parameters;
            foreach (ParameterDefinition xParam in xParams)
            {
                xArgumentSizes.Add(Engine.GetFieldStorageSize(xParam.ParameterType));
            }
            if (!method.IsStatic)
            {
                xArgumentSizes.Insert(0, 4);
            }
            ArgumentSizes = xArgumentSizes.ToArray();
            foreach (ParameterInfo xParam in xParams)
            {
                if (xParam.IsOut)
                {
                    needsCleanup = true;
                    break;
                }
            }
            if (needsCleanup)
            {
                TotalArgumentSize = 0;
                foreach (var xArgSize in ArgumentSizes)
                {
                    TotalArgumentSize += xArgSize;
                }
            }
            // todo: add support for other argument sizes
        }

        public Call(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            mMethodInfo = aMethodInfo;

            var xMethod = instruction.Operand as MethodReference;

            //not the last instruction
            if (instruction.Next != null)
            {
                mNextLabelName = GetInstructionLabel(instruction.Next.Offset);
            }
            else
            {
                mNextLabelName = X86MethodFooterOp.EndOfMethodLabelNameNormal;
            }
            Initialize(xMethod, (uint)instruction.Offset, aMethodInfo.DebugMode);
        }
        public void Assemble(string aMethod, int aArgumentCount)
        {
            if (mTargetMethodInfo.ExtraStackSize > 0)
            {
                new CPUx86.Sub { DestinationReg = CPUx86.Registers.ESP, SourceValue = (uint)mTargetMethodInfo.ExtraStackSize };
            }
            new CPUx86.Call { DestinationLabel = aMethod };
            //if (mResultSize != 0) {
            //new CPUx86.Pop("eax");
            //}
            EmitExceptionLogic(Assembler,
                               mCurrentILOffset,
                               mMethodInfo,
                               mNextLabelName,
                               true,
                               delegate()
                               {
                                   var xResultSize = mTargetMethodInfo.ReturnSize;
                                   if (xResultSize % 4 != 0)
                                   {
                                       xResultSize += 4 - (xResultSize % 4);
                                   }
                                   for (int i = 0; i < xResultSize / 4; i++)
                                   {
                                       new CPUx86.Add { DestinationReg = CPUx86.Registers.ESP, SourceValue = 4 };
                                   }
                               });
            for (int i = 0; i < aArgumentCount; i++)
            {
                Assembler.StackContents.Pop();
            }
            if (mResultSize == 0)
            {
                return;
            }

            Assembler.StackContents.Push(new StackContent((int)mResultSize,
                                                          ((MethodInfo)mTargetMethodInfo.Method).ReturnType));
        }

        protected virtual void HandleDebuggerBreak()
        {
            new CPUx86.Call { DestinationLabel = "DebugStub_Step" };
        }

        public override void DoAssemble()
        {
            if (mIsDebugger_Break)
            {
                HandleDebuggerBreak();
            }
            else
            {
                Assemble(LabelName, ArgumentSizes.Length);
            }
        }
    }
}