using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU
{
    public static class GCImplementationRefs
    {
        public static readonly MethodDefinition AllocNewObjectRef;
        public static readonly MethodDefinition IncRefCountRef;
        public static readonly MethodDefinition DecRefCountRef;

        static GCImplementationRefs()
        {
            RefSetter.SetFields(typeof(GCImplementation), typeof(GCImplementationRefs));
        }
    }
}