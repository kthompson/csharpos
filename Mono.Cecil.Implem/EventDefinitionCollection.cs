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
 * Thu Aug 25 19:01:29 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

	using System;
	using System.Collections;

	using Mono.Cecil;
	using Mono.Cecil.Cil;

	internal class EventDefinitionCollection : IEventDefinitionCollection, ILazyLoadableCollection {

		private IList m_items;
		private TypeDefinition m_container;
		private ReflectionController m_controller;

		private bool m_loaded;

		public event EventDefinitionEventHandler OnEventDefinitionAdded;
		public event EventDefinitionEventHandler OnEventDefinitionRemoved;

		public IEventDefinition this [int index] {
			get {
				return m_items [index] as IEventDefinition;
			}
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

		public bool Loaded {
			get { return m_loaded; }
			set { m_loaded = value; }
		}

		public EventDefinitionCollection (TypeDefinition container)
		{
			m_container = container;
			m_items = new ArrayList ();
		}

		public EventDefinitionCollection (TypeDefinition container, ReflectionController controller) : this (container)
		{
			m_controller = controller;
		}

		public void Add (IEventDefinition value)
		{
			if (OnEventDefinitionAdded != null && !this.Contains (value))
				OnEventDefinitionAdded (this, new EventDefinitionEventArgs (value));
			m_items.Add (value);
		}

		public void Clear ()
		{
			if (OnEventDefinitionRemoved != null)
				foreach (IEventDefinition item in this)
					OnEventDefinitionRemoved (this, new EventDefinitionEventArgs (item));
			m_items.Clear ();
		}

		public bool Contains (IEventDefinition value)
		{
			return m_items.Contains (value);
		}

		public int IndexOf (IEventDefinition value)
		{
			return m_items.IndexOf (value);
		}

		public void Insert (int index, IEventDefinition value)
		{
			if (OnEventDefinitionAdded != null && !this.Contains (value))
				OnEventDefinitionAdded (this, new EventDefinitionEventArgs (value));
			m_items.Insert (index, value);
		}

		public void Remove (IEventDefinition value)
		{
			if (OnEventDefinitionRemoved != null && this.Contains (value))
				OnEventDefinitionRemoved (this, new EventDefinitionEventArgs (value));
			m_items.Remove (value);
		}

		public void RemoveAt (int index)
		{
			if (OnEventDefinitionRemoved != null)
				OnEventDefinitionRemoved (this, new EventDefinitionEventArgs (this [index]));
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

		public void Load ()
		{
			if (m_controller != null && !m_loaded) {
				m_controller.Reader.VisitEventDefinitionCollection (this);
				m_loaded = true;
			}
		}

		public IEventDefinition GetEvent (string name)
		{
			foreach (IEventDefinition evt in this)
				if (evt.Name == name)
					return evt;

			return null;
		}

		public void Accept (IReflectionVisitor visitor)
		{
			visitor.VisitEventDefinitionCollection (this);
		}
	}
}
