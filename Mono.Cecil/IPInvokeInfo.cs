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

namespace Mono.Cecil {

    public interface IPInvokeInfo : IReflectionVisitable {

        IMethodDefinition Method { get; }

        PInvokeAttributes Attributes { get; set; }
        string EntryPoint { get; set; }
        IModuleReference Module { get; set; }
    }
}
