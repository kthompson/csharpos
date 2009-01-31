using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU {
	public static class Utilities {
        
        public static string GetFullName(MethodReference method)
        {
            return GetFullName(method.Resolve());
        }

        public static string GetFullName(MethodDefinition method) {
			StringBuilder xBuilder = new StringBuilder();
			string[] xParts = method.ToString().Split(' ');
			string[] xParts2 = xParts.Skip(1).ToArray();
			
			if (method != null) {
				xBuilder.Append(method.ReturnType.ReturnType.FullName);
			} else {
				if (method != null) {
					xBuilder.Append(typeof(void).FullName);
				} else {
					xBuilder.Append(xParts[0]);
				}
			}
			xBuilder.Append("  ");
			xBuilder.Append(method.DeclaringType.FullName);
			xBuilder.Append(".");
			xBuilder.Append(method.Name);
			xBuilder.Append("(");
			ParameterDefinitionCollection xParams = method.Parameters;
			for (int i = 0; i < xParams.Count; i++) {
				if (xParams[i].Name == "aThis" && i == 0) {
					continue;
				}
				xBuilder.Append(xParams[i].ParameterType.FullName);
				if (i < (xParams.Count - 1)) {
					xBuilder.Append(", ");
				}
			}
			xBuilder.Append(")");
			return xBuilder.ToString();
		}
	}
}
