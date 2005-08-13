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

	using System;

	using Mono.Cecil.Cil;

	public interface IMethodReference : IMethodSignature, IMemberReference, IReflectionVisitable {
	}

	public interface IMethodDefinition : IMemberDefinition, IMethodReference, IHasSecurity, ICustomAttributeProvider {

		MethodAttributes Attributes { get; set; }
		MethodImplAttributes ImplAttributes { get; set; }
		MethodSemanticsAttributes SemanticsAttributes { get; set; }

		bool IsAbstract { get; set; }
		bool IsFinal { get; set; }
		bool IsHideBySignature { get; set; }
		bool IsNewSlot { get; set; }
		bool IsRuntimeSpecialName { get; set; }
		bool IsSpecialName { get; set; }
		bool IsStatic { get; set; }
		bool IsVirtual { get; set; }

		IOverrideCollection Overrides { get; }
		IMethodBody Body { get; }
		IPInvokeInfo PInvokeInfo { get; }

		IMethodBody DefineBody ();

		void DefineOverride (IMethodReference meth);
		void DefineOverride (System.Reflection.MethodInfo meth);

		void DefinePInvokeInfo (string ep, string module, PInvokeAttributes attrs);

		IParameterDefinition DefineParameter (string name, ParamAttributes attributes, ITypeReference type);
		IParameterDefinition DefineParameter (string name, ParamAttributes attributes, Type type);
	}
}
