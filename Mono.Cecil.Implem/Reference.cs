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
    using Mono.Cecil.Signatures;

    internal sealed class Reference : TypeReference, IReference {

        private ITypeReference m_type;

        public override string Name {
            get { return m_type.Name; }
            set { m_type.Name = value; }
        }

        public override string Namespace {
            get { return m_type.Namespace; }
            set { m_type.Namespace = value; }
        }

        public ITypeReference Type {
            get { return m_type; }
            set { m_type = value; }
        }

        public override string FullName {
            get { return string.Concat (base.FullName, "&"); }
        }

        public Reference (ITypeReference type) : base (string.Empty, string.Empty)
        {
            m_type = type;
        }
    }
}
