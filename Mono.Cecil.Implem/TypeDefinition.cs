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

    using System;

    using Mono.Cecil;

    internal sealed class TypeDefinition : ITypeDefinition {

        private string m_name;
        private string m_namespace;
        private TypeAttributes m_attributes;
        private ITypeReference m_baseType;

        private ModuleDefinition m_module;

        private InterfaceCollection m_interfaces;
        private MethodDefinitionCollection m_methods;
        private FieldDefinitionCollection m_fields;
        private EventDefinitionCollection m_events;
        private PropertyDefinitionCollection m_properties;

        private ITypeReference m_declaringType;

        public string Name {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Namespace {
            get { return m_namespace; }
            set { m_namespace = value; }
        }

        public TypeAttributes Attributes {
            get { return m_attributes; }
            set { m_attributes = value; }
        }

        public ITypeReference BaseType {
            get { return m_baseType; }
            set { m_baseType = value; }
        }

        public string FullName {
            get { return Utilities.TypeFullName (this); }
        }

        public ITypeReference DeclaringType {
            get { return m_declaringType; }
            set { m_declaringType = value; }
        }

        public IInterfaceCollection Interfaces {
            get {
                if (m_interfaces == null)
                    m_interfaces = new InterfaceCollection (this);
                return m_interfaces;
            }
        }

        public IMethodDefinitionCollection Methods {
            get {
                if (m_methods == null)
                    m_methods = new MethodDefinitionCollection (this);
                return m_methods;
            }
        }

        public IFieldDefinitionCollection Fields {
            get {
                if (m_fields == null)
                    m_fields = new FieldDefinitionCollection (this);
                return m_fields;
            }
        }

        public IEventDefinitionCollection Events {
            get {
                if (m_events == null)
                    m_events = new EventDefinitionCollection (this);
                return m_events;
            }
        }

        public IPropertyDefinitionCollection Properties {
            get {
                if (m_properties == null)
                    m_properties = new PropertyDefinitionCollection (this);
                return m_properties;
            }
        }

        public ModuleDefinition Module {
            get { return m_module; }
        }

        public TypeDefinition (string name, string ns, TypeAttributes attrs, ModuleDefinition module)
        {
            m_name = name;
            m_namespace = ns;
            m_attributes = attrs;
            m_module = module;
        }

        public void Accept (IReflectionVisitor visitor)
        {
            visitor.Visit (this);

            m_interfaces.Accept (visitor);
            m_fields.Accept (visitor);
            m_properties.Accept (visitor);
            m_events.Accept (visitor);
            m_methods.Accept (visitor);
        }

        public override string ToString ()
        {
            return this.FullName;
        }
    }
}
