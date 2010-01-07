using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Translator.X86
{
    public enum AddressingMethod
    {
        /// <summary>
        /// Direct address. The instruction has no ModR/M byte; the address of the operand is encoded in the instruction; no base register, index register, or scaling factor can be applied (for example, far JMP (EA)).
        /// </summary> 
        A, 
        /// <summary>
        /// Memory addressed by DS:EAX, or by rAX in 64-bit mode (only 0F01C8 MONITOR).
        /// </summary> 
        BA, 
        /// <summary>
        /// Memory addressed by DS:eBX+AL, or by rBX+AL in 64-bit mode (only XLAT). (This code changed from single B in revision 1.00)
        /// </summary> 
        BB, 
        /// <summary>
        /// Memory addressed by DS:eDI or by RDI (only 0FF7 MASKMOVQ and 660FF7 MASKMOVDQU) (This code changed from YD (introduced in 1.00) in revision 1.02)
        /// </summary> 
        BD, 
        /// <summary>
        /// The reg field of the ModR/M byte selects a control register (only MOV (0F20, 0F22)).
        /// </summary> 
        C, 
        /// <summary>
        /// The reg field of the ModR/M byte selects a debug register (only MOV (0F21, 0F23)).
        /// </summary> 
        D, 
        /// <summary>
        /// A ModR/M byte follows the opcode and specifies the operand. The operand is either a general-purpose register or a memory address. If it is a memory address, the address is computed from a segment register and any of the following values: a base register, an index register, a scaling factor, or a displacement.
        /// </summary> 
        E, 
        /// <summary>
        /// (Implies original E). A ModR/M byte follows the opcode and specifies the operand. The operand is either a x87 FPU stack register or a memory address. If it is a memory address, the address is computed from a segment register and any of the following values: a base register, an index register, a scaling factor, or a displacement.
        /// </summary> 
        ES, 
        /// <summary>
        /// (Implies original E). A ModR/M byte follows the opcode and specifies the x87 FPU stack register.
        /// </summary> 
        EST, 
        /// <summary>
        /// rFLAGS register.
        /// </summary> 
        F, 
        /// <summary>
        /// The reg field of the ModR/M byte selects a general register (for example, AX (000)).
        /// </summary> 
        G, 
        /// <summary>
        /// The r/m field of the ModR/M byte always selects a general register, regardless of the mod field (for example, MOV (0F20)).
        /// </summary> 
        H, 
        /// <summary>
        /// Immediate data. The operand value is encoded in subsequent bytes of the instruction.
        /// </summary> 
        I, 
        /// <summary>
        /// The instruction contains a relative offset to be added to the instruction pointer register (for example, JMP (E9), LOOP)).
        /// </summary> 
        J, 
        /// <summary>
        /// The ModR/M byte may refer only to memory: mod != 11bin (BOUND, LEA, CALLF, JMPF, LES, LDS, LSS, LFS, LGS, CMPXCHG8B,CMPXCHG16B, F20FF0 LDDQU).
        /// </summary> 
        M, 
        /// <summary>
        /// The R/M field of the ModR/M byte selects a packed quadword MMX technology register.
        /// </summary> 
        N, 
        /// <summary>
        /// The instruction has no ModR/M byte; the offset of the operand is coded as a word, double word or quad word (depending on address size attribute) in the instruction. No base register, index register, or scaling factor can be applied (only MOV  (A0, A1, A2,A3)).
        /// </summary> 
        O, 
        /// <summary>
        /// The reg field of the ModR/M byte selects a packed quadword MMX technology register.
        /// </summary> 
        P, 
        /// <summary>
        /// A ModR/M byte follows the opcode and specifies the operand. The operand is either an MMX technology register or a memory address. If it is a memory address, the address is computed from a segment register and any of the following values: a base register, an index register, a scaling factor, and a displacement.
        /// </summary> 
        Q, 
        /// <summary>
        /// The mod field of the ModR/M byte may refer only to a general register (only MOV (0F20-0F24, 0F26)).
        /// </summary> 
        R, 
        /// <summary>
        /// The reg field of the ModR/M byte selects a segment register (only MOV (8C, 8E)).
        /// </summary> 
        S, 
        /// <summary>
        /// Stack operand, used by instructions which either push an operand to the stack or pop an operand from the stack. Pop-like instructions are, for example, POP, RET, IRET, LEAVE. Push-like are, for example, PUSH, CALL, INT. No Operand type is provided along with this method because it depends on source/destination operand(s).
        /// </summary> 
        SC, 
        /// <summary>
        /// The reg field of the ModR/M byte selects a test register (only MOV (0F24, 0F26)).
        /// </summary> 
        T, 
        /// <summary>
        /// The R/M field of the ModR/M byte selects a 128-bit XMM register.
        /// </summary> 
        U, 
        /// <summary>
        /// The reg field of the ModR/M byte selects a 128-bit XMM register.
        /// </summary> 
        V, 
        /// <summary>
        /// A ModR/M byte follows the opcode and specifies the operand. The operand is either a 128-bit XMM register or a memory address. If it is a memory address, the address is computed from a segment register and any of the following values: a base register, an index register, a scaling factor, and a displacement
        /// </summary> 
        W, 
        /// <summary>
        /// Memory addressed by the DS:eSI or by RSI (only MOVS, CMPS, OUTS, and LODS). In 64-bit mode, only 64-bit (RSI) and 32-bit (ESI) address sizes are supported. In non-64-bit mode, only 32-bit (ESI) and 16-bit (SI) address sizes are supported.
        /// </summary> 
        X, 
        /// <summary>
        /// Memory addressed by the ES:eDI or by RDI (only MOVS, CMPS, INS, STOS, and SCAS). In 64-bit mode, only 64-bit (RDI) and 32-bit (EDI) address sizes are supported. In non-64-bit mode, only 32-bit (EDI) and 16-bit (DI) address sizes are supported.
        /// </summary> 
        Y, 
        /// <summary>
        /// The instruction has no ModR/M byte; the three least-significant bits of the opcode byte selects a general-purpose register
        /// </summary> 
        Z, 
    }
}
