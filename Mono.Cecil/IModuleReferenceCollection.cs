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

	public interface IModuleReferenceCollection : ICollection, IReflectionStructureVisitable {

		IModuleReference this [int index] { get; }

		IModuleDefinition Container { get; }

		void Add (IModuleReference value);
		void Clear ();
		bool Contains (IModuleReference value);
		int IndexOf (IModuleReference value);
		void Insert (int index, IModuleReference value);
		void Remove (IModuleReference value);
		void RemoveAt (int index);
	}
}
