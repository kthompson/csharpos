using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;


namespace Indy.IL2CPU.IL.X86.CustomImplementations.System
{
    public static class EventHandlerImplRefs
    {
        public static readonly Assembly RuntimeAssemblyDef;
        public static readonly MethodDefinition CtorRef;
        //public static readonly MethodDefinition GetInvokeMethodRef;
        //public static readonly MethodDefinition MulticastInvokeRef;

        static EventHandlerImplRefs()
        {
            RefSetter.SetFields(typeof(EventHandlerImpl), typeof(EventHandlerImplRefs));
        }
    }
}
