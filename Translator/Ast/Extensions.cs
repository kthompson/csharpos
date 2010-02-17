using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Metadata;

namespace Compiler.Ast
{
    public static class Extensions
    {
        public static bool IsSigned (this ITypedCodeNode tcn)
        {
            switch(tcn.ElementType)
            {
                case ElementType.I:
                case ElementType.I1:
                case ElementType.I2:
                case ElementType.I4:
                case ElementType.I8:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsReal(this ITypedCodeNode tcn)
        {
            switch (tcn.ElementType)
            {
                case ElementType.R4:
                case ElementType.R8:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsObject(this ITypedCodeNode tcn)
        {
            return tcn.ElementType == ElementType.Object;
        }

        public static bool IsPointer(this ITypedCodeNode tcn)
        {
            return tcn.ElementType == ElementType.Ptr;
        }
        
    }
}
