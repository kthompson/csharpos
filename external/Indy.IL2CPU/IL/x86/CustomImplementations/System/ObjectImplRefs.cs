using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;


namespace Indy.IL2CPU.IL.X86.CustomImplementations.System
{
    public static class ObjectImplRefs
    {
        public static readonly Assembly RuntimeAssemblyDef;

        static ObjectImplRefs()
        {
            RefSetter.SetFields(typeof(ObjectImpl), typeof(ObjectImplRefs));
        }
    }
}
