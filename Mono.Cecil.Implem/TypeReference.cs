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

    internal class TypeReference : ITypeReference {

        private string m_name;
        private string m_namespace;
        private ITypeReference m_decType;

        public virtual string Name {
            get { return m_name; }
            set { m_name = value; }
        }

        public virtual string Namespace {
            get { return m_namespace; }
            set { m_namespace = value; }
        }

        public virtual ITypeReference DeclaringType {
            get { return m_decType; }
            set { m_decType = value; }
        }

        public virtual string FullName {
            get {
                if (m_decType != null)
                    return string.Concat (m_decType.FullName, "/", m_name);

                if (m_namespace == null || m_namespace.Length == 0)
                    return m_name;

                return string.Concat (m_namespace, ".", m_name);
            }
        }

        public TypeReference (string name, string ns)
        {
            m_name = name;
            m_namespace = ns;
        }

        public virtual void Accept (IReflectionVisitor visitor)
        {
            visitor.Visit (this);
        }

        public override string ToString ()
        {
            return this.FullName;
        }
    }
}
