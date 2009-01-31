using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace Indy.IL2CPU
{
    public static class RuntimeEngineRefs
    {
        public static readonly AssemblyDefinition RuntimeAssemblyDef;
        public static readonly MethodDefinition FinalizeApplicationRef;
        public static readonly MethodDefinition InitializeApplicationRef;
        public static readonly MethodDefinition Heap_AllocNewObjectRef;

        static RuntimeEngineRefs()
        {
            RefSetter.SetFields(typeof(RuntimeEngine), typeof(RuntimeEngineRefs));
        }
    }
}