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
 * Wed Feb 23 03:31:26 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

    using System;
    using System.Collections;
    using System.Collections.Specialized;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    internal class EventDefinitionCollection : IEventDefinitionCollection, ILazyLoadableCollection {

        private IDictionary m_items;
        private TypeDefinition m_container;
        private LazyLoader m_loader;

        private bool m_loaded;

        public IEventDefinition this [string name] {
            get {
                if (m_loader != null)
                    m_loader.ReflectionReader.Visit (this);
                return m_items [name] as IEventDefinition;
            }
            set { m_items [name] = value; }
        }

        public ITypeDefinition Container {
            get { return m_container; }
        }

        public int Count {
            get {
                if (m_loader != null)
                    m_loader.ReflectionReader.Visit (this);
                return m_items.Count;
            }
        }

        public bool IsSynchronized {
            get { return false; }
        }

        public object SyncRoot {
            get { return this; }
        }

        public bool Loaded {
            get { return m_loaded; }
            set { m_loaded = value; }
        }

        public EventDefinitionCollection (TypeDefinition container, LazyLoader loader)
        {
            m_container = container;
            m_loader = loader;
            m_items = new ListDictionary ();
        }

        public void Clear ()
        {
            m_items.Clear ();
        }

        public bool Contains (IEventDefinition value)
        {
            return m_items.Contains (value);
        }

        public void Remove (IEventDefinition value)
        {
            m_items.Remove (value);
        }

        public void CopyTo (Array ary, int index)
        {
            if (m_loader != null)
                m_loader.ReflectionReader.Visit (this);
            m_items.Values.CopyTo (ary, index);
        }

        public IEnumerator GetEnumerator ()
        {
            if (m_loader != null)
                m_loader.ReflectionReader.Visit (this);
            return m_items.Values.GetEnumerator ();
        }

        public void Accept (IReflectionVisitor visitor)
        {
            visitor.Visit (this);
            IEventDefinition [] items = new IEventDefinition [m_items.Count];
            m_items.Values.CopyTo (items, 0);
            for (int i = 0; i < items.Length; i++)
                items [i].Accept (visitor);
        }
    }
}
