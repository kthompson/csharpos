using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU
{
    public static class TypeResolver
    {
        public static readonly Type Void = typeof(void);
        public static readonly TypeDefinition VoidDef = Resolve(Void);
        public static readonly TypeDefinition ObjectDef = Resolve(Object);
        public static Type Object = typeof(object);

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

        public static MethodDefinition GetMethod<T>(string methodName, params Type[] args)
        {
            return GetMethod(typeof(T), methodName, args);
        }

        public static MethodDefinition GetMethod(Type type, string methodName, params Type[] args)
        {
            var method = TypeResolver.Resolve(type).Methods.GetMethod(methodName, args);
            if (method == null)
                throw new ArgumentException("Method not found");

            return method;
        }
    }
}
