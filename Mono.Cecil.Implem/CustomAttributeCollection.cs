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
 * Sun Aug 28 03:04:17 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

	using System;
	using System.Collections;

	using Mono.Cecil;
	using Mono.Cecil.Cil;

	internal class CustomAttributeCollection : ICustomAttributeCollection {

		private IList m_items;
		private ICustomAttributeProvider m_container;

		public event CustomAttributeEventHandler OnCustomAttributeAdded;
		public event CustomAttributeEventHandler OnCustomAttributeRemoved;

		public ICustomAttribute this [int index] {
			get { return m_items [index] as ICustomAttribute; }
			set { m_items [index] = value; }
		}

		public ICustomAttributeProvider Container {
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

		public CustomAttributeCollection (ICustomAttributeProvider container)
		{
			m_container = container;
			m_items = new ArrayList ();
		}

		public void Add (ICustomAttribute value)
		{
			if (OnCustomAttributeAdded != null && !this.Contains (value))
				OnCustomAttributeAdded (this, new CustomAttributeEventArgs (value));
			m_items.Add (value);
		}

		public void Clear ()
		{
			if (OnCustomAttributeRemoved != null)
				foreach (ICustomAttribute item in this)
					OnCustomAttributeRemoved (this, new CustomAttributeEventArgs (item));
			m_items.Clear ();
		}

		public bool Contains (ICustomAttribute value)
		{
			return m_items.Contains (value);
		}

		public int IndexOf (ICustomAttribute value)
		{
			return m_items.IndexOf (value);
		}

		public void Insert (int index, ICustomAttribute value)
		{
			if (OnCustomAttributeAdded != null && !this.Contains (value))
				OnCustomAttributeAdded (this, new CustomAttributeEventArgs (value));
			m_items.Insert (index, value);
		}

		public void Remove (ICustomAttribute value)
		{
			if (OnCustomAttributeRemoved != null && this.Contains (value))
				OnCustomAttributeRemoved (this, new CustomAttributeEventArgs (value));
			m_items.Remove (value);
		}

		public void RemoveAt (int index)
		{
			if (OnCustomAttributeRemoved != null)
				OnCustomAttributeRemoved (this, new CustomAttributeEventArgs (this [index]));
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

		public void Accept (IReflectionVisitor visitor)
		{
			visitor.VisitCustomAttributeCollection (this);
		}
	}
}
