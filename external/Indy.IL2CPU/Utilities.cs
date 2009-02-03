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
					xBuilder.Append(TypeResolver.Void.FullName);
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

        public static int GetArrayRank(this TypeDefinition type)
        {
            throw new NotImplementedException();
        }

        public static TypeReference GetElementType(this TypeDefinition type)
        {
            throw new NotImplementedException();
        }

        public static bool IsArray(this TypeDefinition type)
        {
            throw new NotImplementedException();
        }

        public static InterfaceMapping GetInterfaceMap(this TypeDefinition type, TypeReference inter)
        {
            throw new NotImplementedException();
        }
	}

    
    public struct InterfaceMapping
    {
        // Summary:
        //     Shows the methods that are defined on the interface.
        public MethodInfo[] InterfaceMethods;
        //
        // Summary:
        //     Shows the type that represents the interface.
        public TypeReference InterfaceType;
        //
        // Summary:
        //     Shows the methods that implement the interface.
        public MethodInfo[] TargetMethods;
        //
        // Summary:
        //     Represents the type that was used to create the interface mapping.
        public TypeReference TargetType;
    }
}
