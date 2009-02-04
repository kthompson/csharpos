using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU
{
    public static class TypeResolver
    {
        public static readonly Type Void;
        public static readonly Type Object;
        public static readonly TypeDefinition VoidDef;
        public static readonly TypeDefinition ObjectDef;

        private static readonly IAssemblyResolver Resolver;
        private static readonly Dictionary<Type, TypeDefinition> Types;

        static TypeResolver()
        {
            Resolver = new DefaultAssemblyResolver();
            Types = new Dictionary<Type, TypeDefinition>();
            
            Void = typeof(void);
            Object = typeof(object);
            
            VoidDef = Resolve(Void);
            ObjectDef = Resolve(Object);
        }

        [Obsolete("Please use  Resolve<T>()")]
        public static TypeDefinition Resolve<T>()
        {
            return Resolve(typeof(T));
        }

        [Obsolete("Please use  Resolve<T>()")]
        public static TypeDefinition Resolve(Type t)
        {
            if (Types.ContainsKey(t))
                return Types[t];

            var asm = Resolver.Resolve(t.Assembly.FullName);
            if (asm == null)
                throw new Exception("Assembly not found.");

            foreach (ModuleDefinition module in asm.Modules)
            {
                foreach (TypeDefinition type in module.Types)
                {
                    if (type.FullName == t.FullName)
                    {
                        Types.Add(t, type);
                        return type;
                    }
                }
            }

            throw new Exception("Type not found.");
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
