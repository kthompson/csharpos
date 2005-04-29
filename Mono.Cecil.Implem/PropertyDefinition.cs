/*
 * Copyright (c) 2004 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jb.evain@dotnetguru.org)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

    using Mono.Cecil;

    internal sealed class PropertyDefinition : MemberDefinition, IPropertyDefinition {

        private ITypeReference m_propertyType;
        private ParameterDefinitionCollection m_parameters;
        private PropertyAttributes m_attributes;

        private CustomAttributeCollection m_customAttrs;

        private bool m_semanticLoaded;
        private IMethodDefinition m_getMeth;
        private IMethodDefinition m_setMeth;

        private bool m_constLoaded;
        private object m_const;

        public ITypeReference PropertyType {
            get { return m_propertyType; }
            set { m_propertyType = value; }
        }

        public bool SemanticLoaded {
            get { return m_semanticLoaded; }
            set { m_semanticLoaded = value; }
        }

        public IParameterDefinitionCollection Parameters {
            get {
                if (m_parameters == null) {
                    if (this.GetMethod != null)
                        m_parameters = this.GetMethod.Parameters as ParameterDefinitionCollection;
                    else
                        m_parameters = new ParameterDefinitionCollection (this);
                }
                return m_parameters;
            }
        }

        public PropertyAttributes Attributes {
            get { return m_attributes; }
            set { m_attributes = value; }
        }

        public ICustomAttributeCollection CustomAttributes {
            get {
                if (m_customAttrs == null)
                    m_customAttrs = new CustomAttributeCollection (this, (this.DeclaringType as TypeDefinition).Module.Loader);
                return m_customAttrs;
            }
        }

        public IMethodDefinition GetMethod {
            get {
                ((TypeDefinition)this.DeclaringType).Module.Loader.DetailReader.ReadSemantic (this);
                return m_getMeth;
            }
            set { m_getMeth = value; }
        }

        public IMethodDefinition SetMethod {
            get {
                ((TypeDefinition)this.DeclaringType).Module.Loader.DetailReader.ReadSemantic (this);
                return m_setMeth;
            }
            set { m_setMeth = value; }
        }

        public bool ConstantLoaded {
            get { return m_constLoaded; }
            set { m_constLoaded = value; }
        }

        public object Constant {
            get {
                (this.DeclaringType as TypeDefinition).Module.Loader.DetailReader.ReadConstant (this);
                return m_const;
            }
            set { m_const = value; }
        }

        public PropertyDefinition (string name, TypeDefinition decType, ITypeReference propType, PropertyAttributes attrs) : base (name, decType)
        {
            m_propertyType = propType;
            m_attributes = attrs;
        }

        public ICustomAttribute DefineCustomAttribute (IMethodReference ctor)
        {
            CustomAttribute ca = new CustomAttribute(ctor);
            m_customAttrs.Add (ca);
            return ca;
        }

        public ICustomAttribute DefineCustomAttribute (System.Reflection.ConstructorInfo ctor)
        {
            //TODO: implement this
            return null;
        }

        public void Accept (IReflectionVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
