using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Translator
{
    public class AssemblyCompiler : IAssemblyCompiler, IReflectionVisitor
    {
        #region IAssemblyCompiler Members

        public AssemblyDefinition Assembly { get; private set; }

        #endregion

        #region ICompiler Members

        public void Compile()
        {
            this.Assembly.MainModule.Accept(this);
        }

        #endregion

        #region IReflectionVisitor Members

        void IReflectionVisitor.VisitModuleDefinition(ModuleDefinition module)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitTypeDefinitionCollection(TypeDefinitionCollection types)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitTypeDefinition(TypeDefinition type)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitTypeReferenceCollection(TypeReferenceCollection refs)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitTypeReference(TypeReference type)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitMemberReferenceCollection(MemberReferenceCollection members)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitMemberReference(MemberReference member)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitInterfaceCollection(InterfaceCollection interfaces)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitInterface(TypeReference interf)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitExternTypeCollection(ExternTypeCollection externs)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitExternType(TypeReference externType)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitOverrideCollection(OverrideCollection meth)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitOverride(MethodReference ov)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitNestedTypeCollection(NestedTypeCollection nestedTypes)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitNestedType(TypeDefinition nestedType)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitParameterDefinitionCollection(ParameterDefinitionCollection parameters)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitParameterDefinition(ParameterDefinition parameter)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitMethodDefinitionCollection(MethodDefinitionCollection methods)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitMethodDefinition(MethodDefinition method)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitConstructorCollection(ConstructorCollection ctors)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitConstructor(MethodDefinition ctor)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitPInvokeInfo(PInvokeInfo pinvk)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitEventDefinitionCollection(EventDefinitionCollection events)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitEventDefinition(EventDefinition evt)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitFieldDefinitionCollection(FieldDefinitionCollection fields)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitFieldDefinition(FieldDefinition field)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitPropertyDefinitionCollection(PropertyDefinitionCollection properties)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitPropertyDefinition(PropertyDefinition property)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitSecurityDeclarationCollection(SecurityDeclarationCollection secDecls)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitSecurityDeclaration(SecurityDeclaration secDecl)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitCustomAttributeCollection(CustomAttributeCollection customAttrs)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitCustomAttribute(CustomAttribute customAttr)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitGenericParameterCollection(GenericParameterCollection genparams)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitGenericParameter(GenericParameter genparam)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.VisitMarshalSpec(MarshalSpec marshalSpec)
        {
            throw new NotImplementedException();
        }

        void IReflectionVisitor.TerminateModuleDefinition(ModuleDefinition module)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
