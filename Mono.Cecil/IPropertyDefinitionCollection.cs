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
 * Thu Aug 25 05:37:44 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil {

	using System;
	using System.Collections;

	public class PropertyDefinitionEventArgs : EventArgs {

		private IPropertyDefinition m_item;

		public IPropertyDefinition PropertyDefinition {
			get { return m_item; }
		}

		public PropertyDefinitionEventArgs (IPropertyDefinition item)
		{
			m_item = item;
		}
	}

	public delegate void PropertyDefinitionEventHandler (
		object sender, PropertyDefinitionEventArgs ea);

	public interface IPropertyDefinitionCollection : ICollection, IReflectionVisitable {

		IPropertyDefinition this [int index] { get; }

		ITypeDefinition Container { get; }

		event PropertyDefinitionEventHandler OnPropertyDefinitionAdded;
		event PropertyDefinitionEventHandler OnPropertyDefinitionRemoved;

		void Add (IPropertyDefinition value);
		void Clear ();
		bool Contains (IPropertyDefinition value);
		int IndexOf (IPropertyDefinition value);
		void Insert (int index, IPropertyDefinition value);
		void Remove (IPropertyDefinition value);
		void RemoveAt (int index);

		IPropertyDefinition [] GetProperties (string name);
	}
}
