/*
 * Copyright (c) 2004 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jb.evain@dotnetguru.org)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 *****************************************************************************/

namespace Mono.Cecil {

    using System;

    [Flags]
    internal enum PInvokeAttributes : ushort {
        NoMangle            = 0x0001,    // PInvoke is to use the member name as specified
    
        // Character set
        CharSetMask         = 0x0006,
        CharSetNotSpec      = 0x0000,
        CharSetAnsi         = 0x0002,
        CharSetUnicode      = 0x0004,
        CharSetAuto         = 0x0006,
        SupportsLastError   = 0x0040,    // Information about target function. Not relevant for fields
    
        // Calling convetion
        CallConvMask        = 0x0700,
        CallConvWinapi      = 0x0100,
        CallConvCdecl       = 0x0200,
        CallConvStdCall     = 0x0300,
        CallConvThiscall    = 0x0400,
        CallConvFastcall    = 0x0500
    }
}
