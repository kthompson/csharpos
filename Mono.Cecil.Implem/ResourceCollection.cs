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
 * Tue Jul 05 15:48:32 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

	using System;
	using System.Collections;

	using Mono.Cecil;
	using Mono.Cecil.Cil;

	internal class ResourceCollection : IResourceCollection {

		private IDictionary m_items;
		private IModuleDefinition m_container;

		public IResource this [string name] {
			get { return m_items [name] as IResource; }
			set { m_items [name] = value; }
		}

		public IModuleDefinition Container {
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

		public ResourceCollection (IModuleDefinition container)
		{
			m_container = container;
			m_items = new Hashtable ();
		}

		public void Clear ()
		{
			m_items.Clear ();
		}

		public bool Contains (IResource value)
		{
			return m_items.Contains (value);
		}

		public void Remove (IResource value)
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

		public void Accept (IReflectionStructureVisitor visitor)
		{
			visitor.Visit (this);
			IResource [] items = new IResource [m_items.Count];
			m_items.Values.CopyTo (items, 0);
			for (int i = 0; i < items.Length; i++)
				items [i].Accept (visitor);
		}
	}
}
