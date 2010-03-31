using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Framework;

namespace Compiler.X86
{
    public static class Registers
    {
        public static IRegister Eax = new GenericRegister32("eax", "ax", "ah", "al");
        public static IRegister Ebx = new GenericRegister32("ebx", "bx", "bh", "bl");
        public static IRegister Ecx = new GenericRegister32("ecx", "cx", "ch", "cl");
        public static IRegister Edx = new GenericRegister32("edx", "dx", "dh", "dl");

        public static IRegister Esp = new GenericRegister32("esp", "sp");
        public static IRegister Ebp = new GenericRegister32("ebp", "bp");
        public static IRegister Esi = new GenericRegister32("esi", "si");
        public static IRegister Edi = new GenericRegister32("edi", "di");

        public static IRegister[] All = new[]{ Eax, Ebx, Ecx, Edx, Esp, Ebp, Esi, Edi};
    }
}
