using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Compiler.Framework;
using Mono.Cecil;

namespace Compiler.X86
{
    class GenericRegister32 : RegisterBase
    {
        public GenericRegister32(string name, string name16)
            : base(name, 32, null, new GenericRegister16(name16))
        {

        }

        public GenericRegister32(string name, string name16, string name8H, string name8L)
            : base(name, 32, null, new GenericRegister16(name16, name8H, name8L))
        {

        }

        public override bool CanStore(TypeReference type)
        {
            return (type.IsValueType);
        }
    }

    class GenericRegister16 : RegisterBase
    {
        public GenericRegister16(string name)
            : base(name, 16)
        {

        }

        public GenericRegister16(string name, string high, string low)
            : base(name, 16, new GenericRegister8(high), new GenericRegister8(low))
        {

        }

        public override bool CanStore(TypeReference type)
        {
            return (type.IsValueType);
        }
    }

    class GenericRegister8: RegisterBase
    {
        public GenericRegister8(string name)
            : base(name, 8)
        {

        }

        public override bool CanStore(TypeReference type)
        {
            return (type.IsValueType);
        }
    }
}
