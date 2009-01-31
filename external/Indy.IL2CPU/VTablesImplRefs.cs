using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU
{
    public static class VTablesImplRefs
    {
        public static readonly AssemblyDefinition RuntimeAssemblyDef;
        public static readonly TypeDefinition VTablesImplDef;
        public static readonly MethodDefinition LoadTypeTableRef;
        public static readonly MethodDefinition SetTypeInfoRef;
        public static readonly MethodDefinition SetMethodInfoRef;
        public static readonly MethodDefinition GetMethodAddressForTypeRef;
        public static readonly MethodDefinition IsInstanceRef;

        static VTablesImplRefs()
        {
            RefSetter.SetFields(typeof(VTablesImpl), typeof(VTablesImplRefs));
        }
    }
}
