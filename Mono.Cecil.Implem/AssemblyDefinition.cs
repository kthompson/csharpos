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

namespace Mono.Cecil.Implem {

    using System;

    using Mono.Cecil;
    using Mono.Cecil.Metadata;

    internal class AssemblyDefinition : IAssemblyDefinition {

        private AssemblyName m_asmName;
        private ModuleDefinitionCollection m_modules;

        public IAssemblyName Name {
            get { return m_asmName; }
        }

        public IModuleDefinitionCollection Modules {
            get { return m_modules; }
        }

        public AssemblyDefinition (AssemblyName name)
        {
            if (name == null)
                throw new ArgumentException ("name");

            m_asmName = name;
            m_modules = new ModuleDefinitionCollection (this);
        }

        public void Accept (IReflectionStructureVisitor visitor)
        {
            visitor.Visit (this);

            m_asmName.Accept (visitor);
            m_modules.Accept (visitor);
        }
    }
}

