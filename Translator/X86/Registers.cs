using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.X86
{
    public class Registers
    {

        #region 32bit Registers
        public static readonly Register Eax = new Register("eax", 32);
        public static readonly Register Ebx = new Register("ebx", 32);
        public static readonly Register Ecx = new Register("ecx", 32);
        public static readonly Register Edx = new Register("edx", 32);
        
        public static readonly Register Esi = new Register("esi", 32);
        public static readonly Register Esp = new Register("esp", 32);

        public static readonly Register Edi = new Register("edi", 32);
        public static readonly Register Edp = new Register("edp", 32);
        #endregion
    }
}
