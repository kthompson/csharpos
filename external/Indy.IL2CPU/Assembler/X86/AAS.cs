﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indy.IL2CPU.Assembler.X86
{
    class AAS
    {
        [OpCode("aas")]
        public class AAD : Instruction
        {
            public static void InitializeEncodingData(Instruction.InstructionData aData)
            {
                aData.EncodingOptions.Add(new InstructionData.InstructionEncodingOption
                {
                    OpCode = new byte[] { 0x3F}
                });
            }
        }
    }
}