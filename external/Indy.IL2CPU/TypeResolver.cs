using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU
{
    public static class TypeResolver
    {
        public static TypeDefinition Resolve<T>()
        {
            return Resolve(typeof(T));
        }

        [Obsolete("Please use  Resolve<T>()")]
        public static TypeDefinition Resolve(Type t)
        {
            throw new NotImplementedException();
        }

        public static MethodDefinition GetConstructor<T>()
        {
            var type = Resolve<T>();
            return GetConstructor(type);
        }

        public static MethodDefinition GetConstructor(TypeDefinition type)
        {
            foreach (MethodDefinition method in type.Constructors)
            {
                if (method.Parameters.Count == 0)
                    return method;
            }
            return null;
        }

        public static TypeDefinition Resolve(TypeReference typeRef)
        {
            throw new NotImplementedException();
        }
    }
}
