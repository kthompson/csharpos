/*
 * Copyright (c) 2004, 2005 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jbevain@gmail.com)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 *****************************************************************************/

namespace Mono.Cecil.Cil {

    internal enum MethodHeaders : ushort {
        TinyFormat = 0x2,
        FatFormat = 0x3,
        MoreSects = 0x8,
        InitLocals = 0x10
    }
}
