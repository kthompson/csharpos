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

namespace Mono.Cecil.Signatures {

    using Mono.Cecil;

    internal class CustomAttrib {

        public const ushort StdProlog = 0x0001;

        private IMethodReference m_constructor;

        private ushort m_prolog;
        private FixedArg [] m_fixedArgs;
        private ushort m_numNamed;
        private NamedArg [] m_namedArgs;

        public IMethodReference Constructor {
            get { return m_constructor; }
        }

        public ushort Prolog {
            get { return m_prolog; }
            set { m_prolog = value; }
        }

        public FixedArg [] FixedArgs {
            get { return m_fixedArgs; }
            set { m_fixedArgs = value; }
        }

        public ushort NumNamed {
            get { return m_numNamed; }
            set { m_numNamed = value; }
        }

        public NamedArg [] NamedArgs {
            get { return m_namedArgs; }
            set { m_namedArgs = value; }
        }

        public CustomAttrib (IMethodReference ctor)
        {
            m_constructor = ctor;
        }

        internal class FixedArg {

            private bool m_szArray;
            private uint m_numElem;
            private Elem [] m_elems;

            public bool SzArray {
                get { return m_szArray; }
                set { m_szArray = value; }
            }

            public uint NumElem {
                get { return m_numElem; }
                set { m_numElem = value; }
            }

            public Elem [] Elems {
                get { return m_elems; }
                set { m_elems = value; }
            }
        }

        internal class Elem {

            private bool m_simple;
            private bool m_string;
            private bool m_type;
            private bool m_boxedVt;

            private ElementType m_fieldOrPropType;
            private object m_value;

            private ITypeReference m_elemType;

            public bool Simple {
                get { return m_simple; }
                set { m_simple = value; }
            }

            public bool String {
                get { return m_string; }
                set { m_string = value; }
            }

            public bool Type {
                get { return m_type; }
                set { m_type = value; }
            }

            public bool BoxedValueType {
                get { return m_boxedVt; }
                set { m_boxedVt = value; }
            }

            public ElementType FieldOrPropType {
                get { return m_fieldOrPropType; }
                set { m_fieldOrPropType = value; }
            }

            public object Value {
                get { return m_value; }
                set { m_value = value; }
            }

            public ITypeReference ElemType {
                get { return m_elemType; }
                set { m_elemType = value; }
            }
        }

        internal class NamedArg {

            private bool m_field;
            private bool m_property;

            private ElementType m_fieldOrPropType;
            private string m_fieldOrPropName;
            private FixedArg m_fixedArg;

            public bool Field {
                get { return m_field; }
                set { m_field = value; }
            }

            public bool Property {
                get { return m_property; }
                set { m_property = value; }
            }

            public ElementType FieldOrPropType {
                get { return m_fieldOrPropType; }
                set { m_fieldOrPropType = value; }
            }

            public string FieldOrPropName {
                get { return m_fieldOrPropName; }
                set { m_fieldOrPropName = value; }
            }

            public FixedArg FixedArg {
                get { return m_fixedArg; }
                set { m_fixedArg = value; }
            }
        }
    }
}
