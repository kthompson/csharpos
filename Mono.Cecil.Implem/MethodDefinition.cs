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

    internal sealed class MethodDefinition : MemberDefinition, IMethodDefinition {

        private MethodAttributes m_attributes;
        private MethodImplAttributes m_implAttrs;
        private MethodSemanticsAttributes m_semAttrs;

        private OverrideCollection m_overrides;

        public MethodAttributes Attributes {
            get { return m_attributes; }
            set { m_attributes = value; }
        }

        public MethodImplAttributes ImplAttributes {
            get { return m_implAttrs; }
            set { m_implAttrs = value; }
        }

        public MethodSemanticsAttributes SemanticsAttributes {
            get { return m_semAttrs; }
            set { m_semAttrs = value; }
        }

        public IOverrideCollection Overrides {
            get {
                if (m_overrides == null)
                    m_overrides = new OverrideCollection (this);

                return m_overrides;
            }
        }

        public void Accept (IReflectionVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
