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
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * Sun Jan 23 23:32:20 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

    using System;
    using System.Collections;
    using System.Collections.Specialized;

    using Mono.Cecil;

    internal class MethodDefinitionCollection : IMethodDefinitionCollection {

        private IDictionary m_items;
        private ITypeDefinition m_container;

        public IMethodDefinition this [string name] {
            get { return m_items [name] as IMethodDefinition; }
            set { m_items [name] = value; }
        }

        public ITypeDefinition Container {
            get { return m_container; }
        }

        public int Count {
            get { return m_items.Count; }
        }

        public bool IsSynchronized {
            get { return false; }
        }

        public object SyncRoot {
            get { return this; }
        }

        public MethodDefinitionCollection (ITypeDefinition container)
        {
            m_container = container;
            m_items = new ListDictionary();
        }

        public void Clear ()
        {
            m_items.Clear ();
        }

        public bool Contains (IMethodDefinition value)
        {
            return m_items.Contains (value);
        }

        public void Remove (IMethodDefinition value)
        {
            m_items.Remove (value);
        }

        public void CopyTo (Array ary, int index)
        {
            m_items.Values.CopyTo (ary, index);
        }

        public IEnumerator GetEnumerator ()
        {
            return m_items.Values.GetEnumerator ();
        }

        public void Accept (IReflectionVisitor visitor)
        {
            visitor.Visit (this);
            IMethodDefinition [] items = new IMethodDefinition [m_items.Count];
            m_items.Values.CopyTo (items, 0);
            for (int i = 0; i < items.Length; i++)
                items [i].Accept (visitor);
        }
    }
}
