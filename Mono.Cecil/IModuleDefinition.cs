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

    public interface IModuleDefinition : IReflectionStructureVisitable {

        string Name { get; set; }
        Guid Mvid { get; set; }
        bool Main { get; }

        IAssemblyNameReferenceCollection AssemblyReferences { get; }

        IModuleReferenceCollection ModuleReferences { get; }
        void DefineModuleReference (string module);

        IResourceCollection Resources { get; }
        void DefineEmbeddedResource (string name, ManifestResourceAttributes attributes, byte [] data);
        void DefineLinkedResource (string name, ManifestResourceAttributes attributes, string file);

        ITypeDefinitionCollection Types { get; }
    }
}
