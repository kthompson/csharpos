using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU
{
    public static class RefSetter
    {
        static RefSetter()
        {
            
        }

        public static void SetFields(Type source, Type dest)
        {
            var type = TypeResolver.Resolve(dest);
            foreach (FieldDefinition field in TypeResolver.Resolve(source).Fields)
            {
                if (field.Name.EndsWith("Ref"))
                {
                    MethodDefinition method = type.Methods.GetMethod(field.Name.Substring(0, field.Name.Length - "Ref".Length), new Type[]{});
                    if (method == null)
                    {
                        throw new Exception(string.Format("Method '{0}' not found on {1}!", field.Name.Substring(0, field.Name.Length - "Ref".Length), type.FullName));
                    }
                    field.SetValue(null, method);                    
                }
            }
        }
    }
}
