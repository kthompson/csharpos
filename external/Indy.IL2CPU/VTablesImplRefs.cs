using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU {
	public static class VTablesImplRefs {
		public static readonly AssemblyDefinition RuntimeAssemblyDef;
		public static readonly TypeDefinition VTablesImplDef;
        public static readonly MethodDefinition LoadTypeTableRef;
        public static readonly MethodDefinition SetTypeInfoRef;
        public static readonly MethodDefinition SetMethodInfoRef;
        public static readonly MethodDefinition GetMethodAddressForTypeRef;
        public static readonly MethodDefinition IsInstanceRef;

		static VTablesImplRefs() {
            VTablesImplDef = TypeResolver.Resolve(typeof(VTablesImpl));
			foreach (FieldInfo field in typeof(VTablesImplRefs).GetFields()) {
				if (field.Name.EndsWith("Ref")) {
                    var tempMethod = VTablesImplDef.Methods.GetMethod(field.Name.Substring(0, field.Name.Length - "Ref".Length)).First();
					if (tempMethod == null) {
						throw new Exception("Method '" + field.Name.Substring(0, field.Name.Length - "Ref".Length) + "' not found on RuntimeEngine!");
					}
					field.SetValue(null, tempMethod);
				}
			}
		}
	}
}
