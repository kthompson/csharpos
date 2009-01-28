using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU
{
    public class AbstractReflectionVisitor : AbstractVisitor<AbstractReflectionVisitor>, IReflectionVisitor
    {

        #region IReflectionVisitor Members

        public Action<AbstractReflectionVisitor, ModuleDefinition> TerminateModuleDefinition;
        void IReflectionVisitor.TerminateModuleDefinition(ModuleDefinition module)
        {
            Run(TerminateModuleDefinition, module);
        }

        public Action<AbstractReflectionVisitor, MethodDefinition> VisitConstructor;
        void IReflectionVisitor.VisitConstructor(MethodDefinition ctor)
        {
            Run(VisitConstructor, ctor);
        }

        void IReflectionVisitor.VisitConstructorCollection(ConstructorCollection ctors)
        {
            Run(VisitConstructor, ctors);
        }

        public Action<AbstractReflectionVisitor, CustomAttribute> VisitCustomAttribute;
        void IReflectionVisitor.VisitCustomAttribute(CustomAttribute customAttr)
        {
            Run(VisitCustomAttribute, customAttr);
        }

        void IReflectionVisitor.VisitCustomAttributeCollection(CustomAttributeCollection customAttrs)
        {
            Run(VisitCustomAttribute, customAttrs);
        }

        public Action<AbstractReflectionVisitor, EventDefinition> VisitEventDefinition;
        void IReflectionVisitor.VisitEventDefinition(EventDefinition evt)
        {
            Run(VisitEventDefinition, evt);
        }

        void IReflectionVisitor.VisitEventDefinitionCollection(EventDefinitionCollection events)
        {
            Run(VisitEventDefinition, events);
        }

        public Action<AbstractReflectionVisitor, TypeReference> VisitExternType;
        void IReflectionVisitor.VisitExternType(TypeReference externType)
        {
            Run(VisitExternType, externType);
        }

        void IReflectionVisitor.VisitExternTypeCollection(ExternTypeCollection externs)
        {
            Run(VisitExternType, externs);
        }

        public Action<AbstractReflectionVisitor, FieldDefinition> VisitFieldDefinition;
        void IReflectionVisitor.VisitFieldDefinition(FieldDefinition field)
        {
            Run(VisitFieldDefinition, field);
        }

        void IReflectionVisitor.VisitFieldDefinitionCollection(FieldDefinitionCollection fields)
        {
            Run(VisitFieldDefinition, fields);
        }

        public Action<AbstractReflectionVisitor, GenericParameter> VisitGenericParameter;
        void IReflectionVisitor.VisitGenericParameter(GenericParameter genparam)
        {
            Run(VisitGenericParameter, genparam);
        }

        void IReflectionVisitor.VisitGenericParameterCollection(GenericParameterCollection genparams)
        {
            Run(VisitGenericParameter, genparams);
        }

        public Action<AbstractReflectionVisitor, TypeReference> VisitInterface;
        void IReflectionVisitor.VisitInterface(TypeReference interf)
        {
            Run(VisitInterface, interf);
        }

        void IReflectionVisitor.VisitInterfaceCollection(InterfaceCollection interfaces)
        {
            Run(VisitInterface, interfaces);
        }

        public Action<AbstractReflectionVisitor, MarshalSpec> VisitMarshalSpec;
        void IReflectionVisitor.VisitMarshalSpec(MarshalSpec marshalSpec)
        {
            Run(VisitMarshalSpec, marshalSpec);
        }

        public Action<AbstractReflectionVisitor, MemberReference> VisitMemberReference;
        void IReflectionVisitor.VisitMemberReference(MemberReference member)
        {
            Run(VisitMemberReference, member);
        }

        void IReflectionVisitor.VisitMemberReferenceCollection(MemberReferenceCollection members)
        {
            Run(VisitMemberReference, members);
        }

        public Action<AbstractReflectionVisitor, MethodDefinition> VisitMethodDefinition;
        void IReflectionVisitor.VisitMethodDefinition(MethodDefinition method)
        {
            Run(VisitMethodDefinition, method);
        }

        void IReflectionVisitor.VisitMethodDefinitionCollection(MethodDefinitionCollection methods)
        {
            Run(VisitMethodDefinition, methods);
        }

        public Action<AbstractReflectionVisitor, ModuleDefinition> VisitModuleDefinition;
        void IReflectionVisitor.VisitModuleDefinition(ModuleDefinition module)
        {
            Run(VisitModuleDefinition, module);
        }

        public Action<AbstractReflectionVisitor, TypeDefinition> VisitNestedType;
        void IReflectionVisitor.VisitNestedType(TypeDefinition nestedType)
        {
            Run(VisitNestedType, nestedType);
        }

        void IReflectionVisitor.VisitNestedTypeCollection(NestedTypeCollection nestedTypes)
        {
            Run(VisitNestedType, nestedTypes);
        }

        public Action<AbstractReflectionVisitor, MemberReference> VisitOverride;
        void IReflectionVisitor.VisitOverride(MethodReference ov)
        {
            Run(VisitOverride, ov);
        }

        void IReflectionVisitor.VisitOverrideCollection(OverrideCollection meth)
        {
            Run(VisitOverride, meth);
        }

        public Action<AbstractReflectionVisitor, PInvokeInfo> VisitPInvokeInfo;
        void IReflectionVisitor.VisitPInvokeInfo(PInvokeInfo pinvk)
        {
            Run(VisitPInvokeInfo, pinvk);
        }

        public Action<AbstractReflectionVisitor, ParameterDefinition> VisitParameterDefinition;
        void IReflectionVisitor.VisitParameterDefinition(ParameterDefinition parameter)
        {
            Run(VisitParameterDefinition, parameter);
        }

        void IReflectionVisitor.VisitParameterDefinitionCollection(ParameterDefinitionCollection parameters)
        {
            Run(VisitParameterDefinition, parameters);
        }

        public Action<AbstractReflectionVisitor, PropertyDefinition> VisitPropertyDefinition;
        void IReflectionVisitor.VisitPropertyDefinition(PropertyDefinition property)
        {
            Run(VisitPropertyDefinition, property);
        }

        void IReflectionVisitor.VisitPropertyDefinitionCollection(PropertyDefinitionCollection properties)
        {
            Run(VisitPropertyDefinition, properties);
        }

        public Action<AbstractReflectionVisitor, SecurityDeclaration> VisitSecurityDeclaration;
        void IReflectionVisitor.VisitSecurityDeclaration(SecurityDeclaration secDecl)
        {
            Run(VisitSecurityDeclaration, secDecl);
        }

        void IReflectionVisitor.VisitSecurityDeclarationCollection(SecurityDeclarationCollection secDecls)
        {
            Run(VisitSecurityDeclaration, secDecls);
        }

        public Action<AbstractReflectionVisitor, TypeDefinition> VisitTypeDefinition;
        void IReflectionVisitor.VisitTypeDefinition(TypeDefinition type)
        {
            Run(VisitTypeDefinition, type);
        }

        void IReflectionVisitor.VisitTypeDefinitionCollection(TypeDefinitionCollection types)
        {
            Run(VisitTypeDefinition, types);
        }

        public Action<AbstractReflectionVisitor, TypeReference> VisitTypeReference;
        void IReflectionVisitor.VisitTypeReference(TypeReference type)
        {
            Run(VisitTypeReference, type);
        }

        void IReflectionVisitor.VisitTypeReferenceCollection(TypeReferenceCollection refs)
        {
            Run(VisitTypeReference, refs);
        }

        #endregion

    }
}
