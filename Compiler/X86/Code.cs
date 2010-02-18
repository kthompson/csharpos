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
        /// 
        /// mov{bwl}     imm[8|16|32], r/m[8|16|32]
        /// mov{bwl}     reg[8|16|32], r/m[8|16|32]
        /// mov{bwl}     r/m[8|16|32], reg[8|16|32]
        /// </summary>
        Move,
        Return,
        /// <summary>
        /// Load real/float
        /// 
        /// fld{lst}
        /// </summary>
        LoadReal,
        /// <summary>
        /// Store real/float 
        /// 
        /// fst{ls}
        /// </summary>
        StoreReal,

    }
}
