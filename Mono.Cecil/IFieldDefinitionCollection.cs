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
 * Fri Aug 19 18:04:40 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil {

	using System.Collections;

	public interface IFieldDefinitionCollection : ICollection, IReflectionVisitable {

		IFieldDefinition this [int index] { get; }

		ITypeDefinition Container { get; }

		void Add (IFieldDefinition value);
		void Clear ();
		bool Contains (IFieldDefinition value);
		int IndexOf (IFieldDefinition value);
		void Insert (int index, IFieldDefinition value);
		void Remove (IFieldDefinition value);
		void RemoveAt (int index);
	}
}
