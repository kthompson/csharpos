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
    using System.IO;

    using Mono.Cecil.Binary;
    using Mono.Cecil.Metadata;
    using Mono.Cecil.Implem;

    public class AssemblyFactory {

        private AssemblyFactory ()
        {}

        public static IAssemblyDefinition GetAssembly (string file)
        {
            ImageReader brv = new ImageReader (file);
            StructureReader srv = new StructureReader (brv);
            AssemblyDefinition asm = new AssemblyDefinition (new AssemblyNameDefinition (), srv);

            asm.Accept (srv);
            return asm;
        }
    }
}
