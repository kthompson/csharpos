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

namespace Mono.Cecil.Implem {

    using System.Collections;

    internal interface ILazyLoadable {
        bool Loaded { get; set; }
        void Load ();
    }

    internal interface ILazyLoadableCollection : ILazyLoadable, ICollection {
    }
}
