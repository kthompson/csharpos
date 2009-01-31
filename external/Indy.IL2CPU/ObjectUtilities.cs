using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace Indy.IL2CPU
{
    public static class ObjectUtilities
    {
        public static bool IsDelegate(TypeReference aType)
        {
            if (aType.FullName == "System.Object")
            {
                return false;
            }
            var baseType = aType.Resolve().BaseType;
            if (baseType.FullName == "System.Delegate")
            {
                return true;
            }
            if (baseType.FullName == "System.Object")
            {
                return false;
            }
            return IsDelegate(baseType);
        }

        public static bool IsArray(TypeReference aType)
        {
            if (aType.FullName == "System.Object")
            {
                return false;
            }
            var baseType = aType.Resolve().BaseType;
            if (baseType.FullName == "System.Array")
            {
                return true;
            }
            if (baseType.FullName == "System.Object")
            {
                return false;
            }
            return IsArray(baseType);
        }

        public static uint GetObjectStorageSize(TypeReference aType)
        {
            if (aType == null)
            {
                throw new ArgumentNullException("aType");
            }
            var xTypeInfo = Engine.GetTypeInfo(aType);
            return xTypeInfo.NeedsGC
                       ? xTypeInfo.StorageSize + ObjectImpl.FieldDataOffset
                       : xTypeInfo.StorageSize;
        }
    }
}