using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;


namespace Indy.IL2CPU.IL.X86.CustomImplementations.System
{
    public static class MulticastDelegateImplRefs
    {
        public static readonly Assembly RuntimeAssemblyDef;

        static MulticastDelegateImplRefs()
        {
            RefSetter.SetFields(typeof(MulticastDelegateImpl), typeof(MulticastDelegateImplRefs));
        }
    }
}
