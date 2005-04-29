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

    internal sealed class FieldDefinition : MemberDefinition, IFieldDefinition, IFieldLayoutInfo {

        private ITypeReference m_fieldType;
        private FieldAttributes m_attributes;

        private CustomAttributeCollection m_customAttrs;

        private bool m_layoutLoaded;
        private bool m_hasInfo;
        private uint m_offset;

        private bool m_constLoaded;
        private object m_const;

        private bool m_marshalLoaded;
        private MarshalDesc m_marshalDesc;

        public IFieldLayoutInfo LayoutInfo {
            get {
                (this.DeclaringType as TypeDefinition).Module.Loader.DetailReader.ReadLayout (this);
                return this;
            }
        }

        public bool LayoutLoaded {
            get { return m_layoutLoaded; }
            set { m_layoutLoaded = value; }
        }

        public bool HasLayoutInfo {
            get { return m_hasInfo; }
        }

        public uint Offset {
            get { return m_offset; }
            set {
                m_hasInfo = true;
                m_offset = value;
            }
        }

        public ITypeReference FieldType {
            get { return m_fieldType; }
            set { m_fieldType = value; }
        }

        public FieldAttributes Attributes {
            get { return m_attributes; }
            set { m_attributes = value; }
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

        public ICustomAttributeCollection CustomAttributes {
            get {
                if (m_customAttrs == null)
                    m_customAttrs = new CustomAttributeCollection (this, (this.DeclaringType as TypeDefinition).Module.Loader);
                return m_customAttrs;
            }
        }

        public bool MarshalSpecLoaded {
            get { return m_marshalLoaded; }
            set { m_marshalLoaded = value; }
        }

        public IMarshalSpec MarshalSpec {
            get {
                (this.DeclaringType as TypeDefinition).Module.Loader.DetailReader.ReadMarshalSpec(this);
                return m_marshalDesc;
            }
            set { m_marshalDesc = value as MarshalDesc; }
        }

        public FieldDefinition (string name, TypeDefinition decType, ITypeReference fieldType, FieldAttributes attrs) : base (name, decType)
        {
            m_hasInfo = false;
            m_fieldType = fieldType;
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
            if (m_marshalDesc != null)
                m_marshalDesc.Accept (visitor);
            m_customAttrs.Accept (visitor);
        }
    }
}
