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

    using System.Reflection;

    public interface ICustomAttributeProvider {

        ICustomAttributeCollection CustomAttributes { get; }
        ICustomAttribute DefineCustomAttribute (IMethodReference ctor);
        ICustomAttribute DefineCustomAttribute (ConstructorInfo ctor);
    }
}
