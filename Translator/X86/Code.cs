using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.X86
{
    public enum Code
    {
        /// <summary>
        /// Load a constant to the evaluation stack
        /// </summary>
        Move,
        Return,
    }
}
