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

	using System.Collections;

	public abstract class BaseReflectionVisitor : IReflectionVisitor {

		public virtual void VisitModuleDefinition (IModuleDefinition module)
		{
		}

		public virtual void VisitTypeDefinitionCollection (ITypeDefinitionCollection types)
		{
		}

		public virtual void VisitTypeDefinition (ITypeDefinition type)
		{
		}

		public virtual void VisitTypeReferenceCollection (ITypeReferenceCollection refs)
		{
		}

		public virtual void VisitTypeReference (ITypeReference type)
		{
		}

		public virtual void VisitMemberReferenceCollection (IMemberReferenceCollection members)
		{
		}

		public virtual void VisitMemberReference (IMemberReference member)
		{
		}

		public virtual void VisitInterfaceCollection (IInterfaceCollection interfaces)
		{
		}

		public virtual void VisitInterface (ITypeReference interf)
		{
		}

		public virtual void VisitExternTypeCollection (IExternTypeCollection externs)
		{
		}

		public virtual void VisitExternType (ITypeReference externType)
		{
		}

		public virtual void VisitOverrideCollection (IOverrideCollection meth)
		{
		}

		public virtual void VisitOverride (IMethodReference ov)
		{
		}

		public virtual void VisitNestedTypeCollection (INestedTypeCollection nestedTypes)
		{
		}

		public virtual void VisitNestedType (ITypeDefinition nestedType)
		{
		}

		public virtual void VisitParameterDefinitionCollection (IParameterDefinitionCollection parameters)
		{
		}

		public virtual void VisitParameterDefinition (IParameterDefinition parameter)
		{
		}

		public virtual void VisitMethodDefinitionCollection (IMethodDefinitionCollection methods)
		{
		}

		public virtual void VisitMethodDefinition (IMethodDefinition method)
		{
		}

		public virtual void VisitConstructorCollection (IConstructorCollection ctors)
		{
		}

		public virtual void VisitConstructor (IMethodDefinition ctor)
		{
		}

		public virtual void VisitPInvokeInfo (IPInvokeInfo pinvk)
		{
		}

		public virtual void VisitEventDefinitionCollection (IEventDefinitionCollection events)
		{
		}

		public virtual void VisitEventDefinition (IEventDefinition evt)
		{
		}

		public virtual void VisitFieldDefinitionCollection (IFieldDefinitionCollection fields)
		{
		}

		public virtual void VisitFieldDefinition (IFieldDefinition field)
		{
		}

		public virtual void VisitPropertyDefinitionCollection (IPropertyDefinitionCollection properties)
		{
		}

		public virtual void VisitPropertyDefinition (IPropertyDefinition property)
		{
		}

		public virtual void VisitSecurityDeclarationCollection (ISecurityDeclarationCollection secDecls)
		{
		}

		public virtual void VisitSecurityDeclaration (ISecurityDeclaration secDecl)
		{
		}

		public virtual void VisitCustomAttributeCollection (ICustomAttributeCollection customAttrs)
		{
		}

		public virtual void VisitCustomAttribute (ICustomAttribute customAttr)
		{
		}

		public virtual void VisitMarshalSpec (IMarshalSpec marshalSpec)
		{
		}

		public virtual void TerminateModuleDefinition (IModuleDefinition module)
		{
		}

		protected void VisitCollection (ICollection coll)
		{
			if (coll.Count == 0)
				return;

			foreach (IReflectionVisitable visitable in coll)
				visitable.Accept (this);
		}
	}
}
