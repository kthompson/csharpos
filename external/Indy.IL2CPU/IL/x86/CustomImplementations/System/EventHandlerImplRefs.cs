using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;


namespace Indy.IL2CPU.IL.X86.CustomImplementations.System {
	public static class EventHandlerImplRefs {
		public static readonly Assembly RuntimeAssemblyDef;
		public static readonly MethodDefinition CtorRef;
		//public static readonly MethodDefinition GetInvokeMethodRef;
		//public static readonly MethodDefinition MulticastInvokeRef;

		static EventHandlerImplRefs() {
			Type xType = typeof(EventHandlerImpl);
			foreach (FieldInfo xField in typeof(EventHandlerImplRefs).GetFields()) {
				if (xField.Name.EndsWith("Ref")) {
					MethodDefinition xTempMethod = xType.GetMethod(xField.Name.Substring(0, xField.Name.Length - "Ref".Length));
					if (xTempMethod == null) {
						throw new Exception("Method '" + xField.Name.Substring(0, xField.Name.Length - "Ref".Length) + "' not found on DelegateImpl!");
					}
					xField.SetValue(null, xTempMethod);
				}
			}
		}
	}
}
