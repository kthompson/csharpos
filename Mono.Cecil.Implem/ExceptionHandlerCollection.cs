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
    using System.Collections;

    using Mono.Cecil.Cil;

    internal class ExceptionHandlerCollection : IExceptionHandlerCollection, ICodeVisitable {

        private IList m_items;
        private MethodBody m_container;

        public IExceptionHandler this [int index]
        {
            get { return m_items [index] as IExceptionHandler; }
            set { m_items [index] = value; }
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
        
        public IMethodBody Container {
            get { return m_container; }
        }

        public ExceptionHandlerCollection (MethodBody container)
        {
            m_container = container;
            m_items = new ArrayList ();
        }

        public void Add (IExceptionHandler value)
        {
            m_items.Add (value);
        }

        public void Clear ()
        {
            m_items.Clear ();
        }

        public bool Contains (IExceptionHandler value)
        {
            return m_items.Contains (value);
        }

        public int IndexOf (IExceptionHandler value)
        {
            return m_items.IndexOf (value);
        }

        public void Insert (int index, IExceptionHandler value)
        {
            m_items.Insert (index, value);
        }

        public void Remove (IExceptionHandler value)
        {
            m_items.Remove (value);
        }

        public void RemoveAt (int index)
        {
            m_items.Remove (index);
        }

        public void CopyTo (Array ary, int index)
        {
            m_items.CopyTo (ary, index);
        }

        public IEnumerator GetEnumerator ()
        {
            return m_items.GetEnumerator ();
        }

        public void Accept (ICodeVisitor visitor)
        {
            visitor.Visit (this);

            for (int i = 0; i < m_items.Count; i++)
                this [i].Accept (visitor);
        }
    }
}
