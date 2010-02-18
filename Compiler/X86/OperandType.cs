namespace Compiler.X86
{
    public enum OperandType
    {
        /// <summary>
        /// Two one-word operands in memory or two double-word operands in memory, depending on operand-size attribute (only BOUND).
        /// </summary> 
        a, 
        /// <summary>
        /// Byte, regardless of operand-size attribute.
        /// </summary> 
        b, 
        /// <summary>
        /// Packed-BCD. Only x87 FPU instructions (for example, FBLD).
        /// </summary> 
        bcd, 
        /// <summary>
        /// Byte, sign-extended to the size of the destination operand.
        /// </summary> 
        bs, 
        /// <summary>
        /// (Byte, sign-extended to 64 bits.)
        /// </summary> 
        bsq, 
        /// <summary>
        /// Byte, sign-extended to the size of the stack pointer (for example, PUSH (6A)).
        /// </summary> 
        bss, 
        /// <summary>
        /// Byte or word, depending on operand-size attribute. (unused even by Intel?)
        /// </summary> 
        c, 
        /// <summary>
        /// Doubleword, regardless of operand-size attribute.
        /// </summary> 
        d, 
        /// <summary>
        /// Doubleword-integer. Only x87 FPU instructions (for example, FIADD).
        /// </summary> 
        di, 
        /// <summary>
        /// Double-quadword, regardless of operand-size attribute (for example, CMPXCHG16B).
        /// </summary> 
        dq, 
        /// <summary>
        /// Doubleword, or quadword, promoted by REX.W in 64-bit mode (for example, MOVSXD).
        /// </summary> 
        dqp, 
        /// <summary>
        /// Double-real. Only x87 FPU instructions (for example, FADD).
        /// </summary> 
        dr, 
        /// <summary>
        /// Doubleword, sign-extended to 64 bits (for example, CALL (E8).
        /// </summary> 
        ds, 
        /// <summary>
        /// x87 FPU environment (for example, FSTENV).
        /// </summary> 
        e, 
        /// <summary>
        /// Extended-real. Only x87 FPU instructions (for example, FLD).
        /// </summary> 
        er, 
        /// <summary>
        /// 32-bit or 48-bit pointer, depending on operand-size attribute (for example, CALLF (9A).
        /// </summary> 
        p, 
        /// <summary>
        /// 128-bit packed double-precision floating-point data.
        /// </summary> 
        pd, 
        /// <summary>
        /// Quadword MMX technology data.
        /// </summary> 
        pi, 
        /// <summary>
        /// 128-bit packed single-precision floating-point data.
        /// </summary> 
        ps, 
        /// <summary>
        /// 64-bit packed single-precision floating-point data.
        /// </summary> 
        psq, 
        /// <summary>
        /// (80-bit far pointer.)
        /// </summary> 
        pt, 
        /// <summary>
        /// 32-bit or 48-bit pointer, depending on operand-size attribute, or 80-bit far pointer, promoted by REX.W in 64-bit mode (for example, CALLF (FF /3)).
        /// </summary> 
        ptp, 
        /// <summary>
        /// Quadword, regardless of operand-size attribute (for example, CALL (FF /2)).
        /// </summary> 
        q, 
        /// <summary>
        /// Qword-integer. Only x87 FPU instructions (for example, FILD).
        /// </summary> 
        qi, 
        /// <summary>
        /// Quadword, promoted by REX.W (for example, IRETQ).
        /// </summary> 
        qp, 
        /// <summary>
        /// 6-byte pseudo-descriptor, or 10-byte pseudo-descriptor in 64-bit mode (for example, SGDT).
        /// </summary> 
        s, 
        /// <summary>
        /// Scalar element of a 128-bit packed double-precision floating data.
        /// </summary> 
        sd, 
        /// <summary>
        /// Doubleword integer register (e. g., eax). (unused even by Intel?)
        /// </summary> 
        si, 
        /// <summary>
        /// Single-real. Only x87 FPU instructions (for example, FADD).
        /// </summary> 
        sr, 
        /// <summary>
        /// Scalar element of a 128-bit packed single-precision floating data.
        /// </summary> 
        ss, 
        /// <summary>
        /// x87 FPU state (for example, FSAVE).
        /// </summary> 
        st, 
        /// <summary>
        /// x87 FPU and SIMD state (FXSAVE and FXRSTOR).
        /// </summary> 
        stx, 
        /// <summary>
        /// 10-byte far pointer.
        /// </summary> 
        t, 
        /// <summary>
        /// Word or doubleword, depending on operand-size attribute (for example, INC (40), PUSH (50)).
        /// </summary> 
        v, 
        /// <summary>
        /// Word or doubleword, depending on operand-size attribute, or doubleword, sign-extended to 64 bits for 64-bit operand size.
        /// </summary> 
        vds, 
        /// <summary>
        /// Quadword (default) or word if operand-size prefix is used (for example, PUSH (50)).
        /// </summary> 
        vq, 
        /// <summary>
        /// Word or doubleword, depending on operand-size attribute, or quadword, promoted by REX.W in 64-bit mode.
        /// </summary> 
        vqp, 
        /// <summary>
        /// Word or doubleword sign extended to the size of the stack pointer (for example, PUSH (68)).
        /// </summary> 
        vs, 
        /// <summary>
        /// Word, regardless of operand-size attribute (for example, ENTER).
        /// </summary> 
        w, 
        /// <summary>
        /// Word-integer. Only x87 FPU instructions (for example, FIADD).
        /// </summary> 
        wi, 
    }
}


