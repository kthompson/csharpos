using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Mono.Cecil;

namespace Indy.IL2CPU
{
    public static class ObjectImplRefs
    {
        static ObjectImplRefs()
        {
            RefSetter.SetFields(typeof(object), typeof(ObjectImplRefs));
        }

        public static readonly MethodDefinition Object_Ctor;
    }
}