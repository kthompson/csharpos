using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;

namespace System {
	public static class Extensions {
		public static string GetFullName(this MethodDefinition method) {
			StringBuilder builder = new StringBuilder();
			string[] xParts = method.ToString().Split(' ');
			string[] xParts2 = xParts.Skip(1).ToArray();
            
			if (method.IsConstructor) {
				builder.Append(xParts[0]);
			}else{
                builder.Append(method.ReturnType.ReturnType.FullName);
            }
			builder.Append("  ");
			builder.Append(method.DeclaringType.FullName);
			builder.Append(".");
			builder.Append(method.Name);
			builder.Append("(");
			ParameterDefinitionCollection xParams = method.Parameters;
			for (int i = 0; i < xParams.Count; i++) {
				if (xParams[i].Name == "aThis" && i == 0) {
					continue;
				}
				builder.Append(xParams[i].ParameterType.FullName);
				if (i < (xParams.Count - 1)) {
					builder.Append(", ");
				}
			}
			builder.Append(")");
			return builder.ToString();
		}

		public static string GetFullName(this FieldReference aField) {
			return aField.FieldType.FullName + " " + aField.DeclaringType.FullName + "." + aField.Name;
		}
	}
}