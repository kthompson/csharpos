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
 * Sun Jan 30 19:09:20 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

    using System;
    using System.Collections;
    using System.Collections.Specialized;

    using Mono.Cecil;

    internal class EventDefinitionCollection : IEventDefinitionCollection, ILazyLoadableCollection {

        private IDictionary m_items;
        private TypeDefinition m_container;

        private bool m_loaded;

        public IEventDefinition this [string name] {
            get {
                m_container.Module.Loader.ReflectionReader.Visit (this);
                return m_items [name] as IEventDefinition;
            }
            set { m_items [name] = value; }
        }

        public ITypeDefinition Container {
            get { return m_container; }
        }

        public int Count {
            get {
                m_container.Module.Loader.ReflectionReader.Visit (this);
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

        public EventDefinitionCollection (TypeDefinition container)
        {
            m_container = container;
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
            m_container.Module.Loader.ReflectionReader.Visit (this);
            m_items.Values.CopyTo (ary, index);
        }

        public IEnumerator GetEnumerator ()
        {
            m_container.Module.Loader.ReflectionReader.Visit (this);
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
