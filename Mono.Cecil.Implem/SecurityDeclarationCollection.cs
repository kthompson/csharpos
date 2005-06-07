/*
 * Copyright (c) 2004, 2005 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jbevain@gmail.com)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * Sun Jun 05 22:09:40 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

    using System;
    using System.Collections;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    internal class SecurityDeclarationCollection : ISecurityDeclarationCollection, ILazyLoadableCollection {

        private IList m_items;
        private IHasSecurity m_container;
        private ReflectionController m_controller;

        private bool m_loaded;

        public ISecurityDeclaration this [int index] {
            get {
                Load ();
                return m_items [index] as ISecurityDeclaration;
            }
            set { m_items [index] = value; }
        }

        public IHasSecurity Container {
            get { return m_container; }
        }

        public int Count {
            get {
                Load ();
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

        public SecurityDeclarationCollection (IHasSecurity container)
        {
            m_container = container;
            m_items = new ArrayList ();
        }        

        public SecurityDeclarationCollection (IHasSecurity container, ReflectionController controller) : this (container)
        {
            m_controller = controller;
        }
        
        public void Add (ISecurityDeclaration value)
        {
            m_items.Add (value);
        }

        public void Clear ()
        {
            m_items.Clear ();
        }

        public bool Contains (ISecurityDeclaration value)
        {
            return m_items.Contains (value);
        }
        
        public int IndexOf (ISecurityDeclaration value)
        {
            Load ();
            return m_items.IndexOf (value);
        }

        public void Insert (int index, ISecurityDeclaration value)
        {
            m_items.Insert (index, value);
        }

        public void Remove (ISecurityDeclaration value)
        {
            m_items.Remove (value);
        }

        public void RemoveAt (int index)
        {
            m_items.Remove (index);
        }

        public void CopyTo (Array ary, int index)
        {
            Load ();
            m_items.CopyTo (ary, index);
        }

        public IEnumerator GetEnumerator ()
        {
            Load ();
            return m_items.GetEnumerator ();
        }
        
        public void Load ()
        {
            if (m_controller != null && !m_loaded) {
                m_controller.Reader.Visit (this);
                m_loaded = true;
            }
        }

        public void Accept (IReflectionVisitor visitor)
        {
            visitor.Visit (this);
            ISecurityDeclaration [] items = new ISecurityDeclaration [m_items.Count];
            m_items.CopyTo (items, 0);
            for (int i = 0; i < items.Length; i++)
                items [i].Accept (visitor);
        }
    }
}
