using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU.CustomImplementation.System
{
    public static class ArrayImplRefs
    {
        static ArrayImplRefs()
        {
            RefSetter.SetFields(typeof(ArrayImpl), typeof(ArrayImplRefs));
        }

        //public static readonly MethodDefinition InitArrayWithReferenceTypesRef;
    }
}
