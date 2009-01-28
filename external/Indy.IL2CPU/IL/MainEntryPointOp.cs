using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Mono.Cecil;

namespace Indy.IL2CPU.IL {
	public abstract class MainEntryPointOp: Op {
		protected MainEntryPointOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
			: base(instruction, aMethodInfo) {
		}

		public abstract void Enter(string aName);
		public abstract void Exit();
		public abstract void Push(uint aValue);
        public abstract void Call(MethodDefinition aMethod);
		public abstract void Call(string aLabelName);

		public override void DoAssemble() {
			throw new NotImplementedException();
		}
	}
}
