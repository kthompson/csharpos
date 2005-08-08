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

	public interface IReflectionVisitor {

		void Visit (ITypeDefinitionCollection types);
		void Visit (ITypeDefinition type);
		void Visit (ITypeReferenceCollection refs);
		void Visit (ITypeReference type);
		void Visit (IInterfaceCollection interfaces);
		void Visit (IExternTypeCollection externs);
		void Visit (IOverrideCollection meth);
		void Visit (INestedTypeCollection nestedTypes);
		void Visit (IParameterDefinitionCollection parameters);
		void Visit (IParameterDefinition parameter);
		void Visit (IMethodDefinitionCollection methods);
		void Visit (IMethodDefinition method);
		void Visit (IPInvokeInfo pinvk);
		void Visit (IEventDefinitionCollection events);
		void Visit (IEventDefinition evt);
		void Visit (IFieldDefinitionCollection fields);
		void Visit (IFieldDefinition field);
		void Visit (IPropertyDefinitionCollection properties);
		void Visit (IPropertyDefinition property);
		void Visit (ISecurityDeclarationCollection secDecls);
		void Visit (ISecurityDeclaration secDecl);
		void Visit (ICustomAttributeCollection customAttrs);
		void Visit (ICustomAttribute customAttr);
		void Visit (IMarshalSpec marshalSpec);

		void Terminate (ITypeDefinitionCollection types);
	}
}
