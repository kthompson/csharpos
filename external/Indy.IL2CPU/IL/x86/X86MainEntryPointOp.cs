using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CPU = Indy.IL2CPU.Assembler;
using CPUx86 = Indy.IL2CPU.Assembler.X86;

using System.Reflection;
using Mono.Cecil;

namespace Indy.IL2CPU.IL.X86
{
    public class X86MainEntryPointOp : MainEntryPointOp
    {
        private string mMethodName;
        public X86MainEntryPointOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
        }

        public override void Push(uint aValue)
        {
            new CPUx86.Push { DestinationValue = aValue };
        }

        private int xLabelId = 0;

        public override void Call(MethodDefinition method)
        {
            Engine.QueueMethod(method);
            Call(CPU.Label.GenerateLabelName(method));
            new CPUx86.Test { DestinationReg = CPUx86.Registers.ECX, SourceValue = 2 };
            string xLabel = ".Call_Part2_" + xLabelId++.ToString();
            new CPUx86.ConditionalJump { Condition = CPUx86.ConditionalTestEnum.Equal, DestinationLabel = xLabel };
            //new CPUx86.Call("_CODE_REQUESTED_BREAK_");
            Engine.QueueMethod(Engine.GetMethodDefinition(TypeResolver.Resolve<Assembler.Assembler>(), "PrintException"));
            new CPUx86.Call { DestinationLabel = CPU.Label.GenerateLabelName(Engine.GetMethodDefinition(TypeResolver.Resolve<Assembler.Assembler>(), "PrintException")) };
            new CPU.Label(xLabel);

            if (method != null)
            {
                if (!method.ReturnType.ReturnType.FullName.StartsWith("System.Void"))
                {
                    new CPUx86.Push { DestinationReg = CPUx86.Registers.EAX };
                }
            }
        }

        public override void Call(string aLabelName)
        {
            new CPU.Label(mMethodName + "___" + aLabelName);
            new CPUx86.Call { DestinationLabel = aLabelName };
        }

        public override void Enter(string aName)
        {
            X86MethodHeaderOp.AssembleHeader(Assembler, aName, new MethodInformation.Variable[0], new MethodInformation.Argument[0], false, false);
            mMethodName = aName;
        }

        public override void Exit()
        {
            X86MethodFooterOp.AssembleFooter(0, Assembler, new MethodInformation.Variable[0], new MethodInformation.Argument[0], 0, false, false, 0);
        }
    }
}
