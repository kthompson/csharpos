using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU.CustomImplementation.System
{
    public static class StringImplRefs
    {
        static StringImplRefs()
        {
            RefSetter.SetFields(typeof(StringImpl), typeof(StringImplRefs));
        }

        //public static readonly MethodDefinition GetStorageMetalRef;
        //public static readonly MethodDefinition GetStorageNormalRef;
        public static readonly MethodDefinition GetStorage_ImplRef;
    }
}