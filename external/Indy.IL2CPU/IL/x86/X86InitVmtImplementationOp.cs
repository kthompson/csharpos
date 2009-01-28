﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using CPU = Indy.IL2CPU.Assembler;
using CPUx86 = Indy.IL2CPU.Assembler.X86;
using System.Reflection;
using Indy.IL2CPU.Assembler;
using Mono.Cecil;

namespace Indy.IL2CPU.IL.X86 {
	public class X86InitVmtImplementationOp: InitVmtImplementationOp {
		public X86InitVmtImplementationOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
		}

		protected override void Push(uint aValue) {
            new CPUx86.Push { DestinationValue = aValue };
		}

        protected override void Push(string aLabelName) {
            new CPUx86.Push { DestinationRef = new ElementReference(aLabelName) };
        }

		protected override void Call(MethodDefinition aMethod) {
            new CPUx86.Call { DestinationLabel = CPU.Label.GenerateLabelName(aMethod) };
		}
	}
}
