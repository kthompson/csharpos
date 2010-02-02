using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
using System.Reflection;

namespace VirtualExecutionSystem
{
    public class MethodFinder : BaseStructureVisitor, IReflectionVisitor
    {
        

        public TypeDefinition Type { get; private set; }
        public MethodDefinition Method { get; private set; }
        public bool Found { get; private set; }

        private Type _type;
        private MethodInfo _method;

        public MethodFinder(MethodInfo method)
        {
            _method = method;
            _type = method.DeclaringType;
        }

        public override void VisitModuleDefinition(ModuleDefinition module)
        {
            module.Types.Accept(this);
        }

        public override void VisitModuleDefinitionCollection(ModuleDefinitionCollection modules)
        {
            for (int i = 0; i < modules.Count; i++)
                VisitModuleDefinition(modules[i]);
        }

        public override void TerminateAssemblyDefinition(AssemblyDefinition asm)
        {
            this.Found = (this.Type != null && this.Method != null);
        }

        #region IReflectionVisitor Members

        public void TerminateModuleDefinition(ModuleDefinition module)
        {
        }

        public void VisitConstructor(MethodDefinition ctor)
        {
        }

        public void VisitConstructorCollection(ConstructorCollection ctors)
        {
        }

        public void VisitCustomAttribute(CustomAttribute customAttr)
        {
        }

        public void VisitCustomAttributeCollection(CustomAttributeCollection customAttrs)
        {
        }

        public void VisitEventDefinition(EventDefinition evt)
        {
        }

        public void VisitEventDefinitionCollection(EventDefinitionCollection events)
        {
        }

        public void VisitExternType(TypeReference externType)
        {
        }

        public void VisitExternTypeCollection(ExternTypeCollection externs)
        {
        }

        public void VisitFieldDefinition(FieldDefinition field)
        {
        }

        public void VisitFieldDefinitionCollection(FieldDefinitionCollection fields)
        {
        }

        public void VisitGenericParameter(GenericParameter genparam)
        {
        }

        public void VisitGenericParameterCollection(GenericParameterCollection genparams)
        {
        }

        public void VisitInterface(TypeReference interf)
        {
        }

        public void VisitInterfaceCollection(InterfaceCollection interfaces)
        {
        }

        public void VisitMarshalSpec(MarshalSpec marshalSpec)
        {
        }

        public void VisitMemberReference(MemberReference member)
        {
        }

        public void VisitMemberReferenceCollection(MemberReferenceCollection members)
        {
        }

        public void VisitMethodDefinition(MethodDefinition method)
        {
            if (this.Method != null) return;

            if (!(_method.Name == method.Name &&
               _method.ReturnType.FullName == method.ReturnType.ReturnType.FullName &&
               _method.IsStatic == method.IsStatic &&
               _method.IsVirtual == method.IsVirtual))
                return;

            //check params
            var param = _method.GetParameters();
            var param2 = method.Parameters;

            if (param2.Count != param.Length) return;
            for (int i = 0; i < param2.Count; i++)
            {
                if (!(param2[i].Name == param[i].Name &&
                    param[i].IsIn == param2[i].IsIn &&
                    param[i].ParameterType.FullName == param2[i].ParameterType.FullName))
                    return;
            }

            this.Method = method;
        }

        public void VisitMethodDefinitionCollection(MethodDefinitionCollection methods)
        {
            if (this.Type == null) return;
            for (int i = 0; i < methods.Count; i++)
                VisitMethodDefinition(methods[i]);
        }

        public void VisitNestedType(TypeDefinition nestedType)
        {
        }

        public void VisitNestedTypeCollection(NestedTypeCollection nestedTypes)
        {
        }

        public void VisitOverride(MethodReference ov)
        {
        }

        public void VisitOverrideCollection(OverrideCollection meth)
        {
        }

        public void VisitPInvokeInfo(PInvokeInfo pinvk)
        {
        }

        public void VisitParameterDefinition(ParameterDefinition parameter)
        {
        }

        public void VisitParameterDefinitionCollection(ParameterDefinitionCollection parameters)
        {
        }

        public void VisitPropertyDefinition(PropertyDefinition property)
        {
        }

        public void VisitPropertyDefinitionCollection(PropertyDefinitionCollection properties)
        {
        }

        public void VisitSecurityDeclaration(SecurityDeclaration secDecl)
        {
        }

        public void VisitSecurityDeclarationCollection(SecurityDeclarationCollection secDecls)
        {
        }

        public void VisitTypeDefinition(TypeDefinition type)
        {
            if (this.Type != null) return;
            if (_type.FullName != type.FullName) return;

            this.Type = type;
            this.Type.Accept(this);
        }

        public void VisitTypeDefinitionCollection(TypeDefinitionCollection types)
        {
            foreach (TypeDefinition item in types)
            {
                VisitTypeDefinition(item);
            }
                
        }

        public void VisitTypeReference(TypeReference type)
        {
        }

        public void VisitTypeReferenceCollection(TypeReferenceCollection refs)
        {
        }

        #endregion
    }
}
