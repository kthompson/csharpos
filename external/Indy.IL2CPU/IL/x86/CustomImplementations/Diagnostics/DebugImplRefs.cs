using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;


namespace Indy.IL2CPU.IL.X86LinqTest.CustomImplementations.System.Diagnostics
{
    public static class DebugImplRefs
    {
        public static readonly MethodDefinition WriteLineRef;
        public static readonly MethodDefinition WriteLineIfRef;
        public static readonly Assembly RuntimeAssemblyDef;

        static DebugImplRefs()
        {
            RefSetter.SetFields(typeof(DebugImpl), typeof(DebugImplRefs));
        }
    }
}