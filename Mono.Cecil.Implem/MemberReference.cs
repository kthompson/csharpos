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

    using Mono.Cecil;

    internal abstract class MemberReference : IMemberReference {

        private string m_name;
        private ITypeReference m_decType;

        public string Name {
            get { return m_name; }
            set { m_name = value; }
        }

        public ITypeReference DeclaringType {
            get { return m_decType; }
        }

        public MemberReference (string name, ITypeReference declaringType)
        {
            m_name = name;
            m_decType = declaringType;
        }
    }
}
