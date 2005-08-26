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
 *****************************************************************************/

namespace Mono.Cecil {

	public interface IAssemblyDefinition : ICustomAttributeProvider, IHasSecurity, IReflectionStructureVisitable {

		IAssemblyNameDefinition Name { get; }
		TargetRuntime Runtime { get; set; }

		IModuleDefinitionCollection Modules { get; }
		IModuleDefinition MainModule { get; }

		IMethodDefinition EntryPoint { get; set; }

		IReflectionStructureFactories Factories { get; }
	}
}
