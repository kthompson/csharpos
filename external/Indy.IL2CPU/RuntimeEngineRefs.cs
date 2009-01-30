using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace Indy.IL2CPU {
	public static class RuntimeEngineRefs {
        public static readonly AssemblyDefinition RuntimeAssemblyDef;
		public static readonly MethodDefinition FinalizeApplicationRef;
        public static readonly MethodDefinition InitializeApplicationRef;
        public static readonly MethodDefinition Heap_AllocNewObjectRef;

		static RuntimeEngineRefs() {
			Type xType = typeof(RuntimeEngine);
			foreach (FieldInfo xField in typeof(RuntimeEngineRefs).GetFields()) {
				if (xField.Name.EndsWith("Ref")) {
					MethodDefinition xTempMethod = xType.GetMethod(xField.Name.Substring(0, xField.Name.Length - "Ref".Length));
					if (xTempMethod == null) {
						throw new Exception("Method '" + xField.Name.Substring(0, xField.Name.Length - "Ref".Length) + "' not found on RuntimeEngine!");
					}
					xField.SetValue(null, xTempMethod);
				}
			}
		}
	}
}