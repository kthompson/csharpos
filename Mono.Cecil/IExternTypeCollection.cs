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
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * Sat May 14 20:51:46 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil {

    using System.Collections;

    public interface IExternTypeCollection : ICollection, IReflectionVisitable {

        ITypeReference this [string name] { get; }

        IModuleDefinition Container { get; }

        void Clear ();
        bool Contains (ITypeReference value);
        void Remove (ITypeReference value);
    }
}
