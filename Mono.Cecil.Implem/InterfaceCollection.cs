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
 * Sun Aug 28 03:04:16 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

	using System;
	using System.Collections;

	using Mono.Cecil;
	using Mono.Cecil.Cil;

	internal class InterfaceCollection : IInterfaceCollection {

		private IList m_items;
		private TypeDefinition m_container;

		public event InterfaceEventHandler OnInterfaceAdded;
		public event InterfaceEventHandler OnInterfaceRemoved;

		public ITypeReference this [int index] {
			get { return m_items [index] as ITypeReference; }
			set { m_items [index] = value; }
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

		public InterfaceCollection (TypeDefinition container)
		{
			m_container = container;
			m_items = new ArrayList ();
		}

		public void Add (ITypeReference value)
		{
			if (OnInterfaceAdded != null && !this.Contains (value))
				OnInterfaceAdded (this, new InterfaceEventArgs (value));
			m_items.Add (value);
		}

		public void Clear ()
		{
			if (OnInterfaceRemoved != null)
				foreach (ITypeReference item in this)
					OnInterfaceRemoved (this, new InterfaceEventArgs (item));
			m_items.Clear ();
		}

		public bool Contains (ITypeReference value)
		{
			return m_items.Contains (value);
		}

		public int IndexOf (ITypeReference value)
		{
			return m_items.IndexOf (value);
		}

		public void Insert (int index, ITypeReference value)
		{
			if (OnInterfaceAdded != null && !this.Contains (value))
				OnInterfaceAdded (this, new InterfaceEventArgs (value));
			m_items.Insert (index, value);
		}

		public void Remove (ITypeReference value)
		{
			if (OnInterfaceRemoved != null && this.Contains (value))
				OnInterfaceRemoved (this, new InterfaceEventArgs (value));
			m_items.Remove (value);
		}

		public void RemoveAt (int index)
		{
			if (OnInterfaceRemoved != null)
				OnInterfaceRemoved (this, new InterfaceEventArgs (this [index]));
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
			visitor.VisitInterfaceCollection (this);
		}
	}
}
