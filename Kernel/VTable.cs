using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Kernel
{
    public class VTable : BaseReflectionVisitor
    {
        private List<VTableEntry> _entries;
        public TypeDefinition Type { get; private set; }

        public VTable()
        {
            _entries = new List<VTableEntry>();
        }

        public override void VisitTypeDefinition(TypeDefinition type)
        {
            if (this.Type != null)
                throw new NotSupportedException();

            this.Type = type;
        }

        public override void VisitMethodDefinitionCollection(MethodDefinitionCollection methods)
        {
            for (int i = 0; i < methods.Count; i++)
                VisitMethodDefinition(methods[i]);
        }

        public override void VisitMethodDefinition(MethodDefinition method)
        {
            _entries.Add(new VTableEntry(method));
        }

        public override void VisitConstructorCollection(ConstructorCollection ctors)
        {
            for (int i = 0; i < ctors.Count; i++)
                VisitConstructor(ctors[i]);
        }

        public override void VisitConstructor(MethodDefinition ctor)
        {
            VisitMethodDefinition(ctor);
        }

        public class VTableEntry
        {
            public int BaseType
            public uint Address { get; private set; }
            public bool IsCompiled { get; private set; }

            public VTableEntry(MethodDefinition method)
            {
                this.Method = method;
            }

            private void Compile()
            {
                this.IsCompiled = true;
                throw new NotImplementedException();
            }

            public void Execute()
            {
                if (!this.IsCompiled)
                    this.Compile();

                throw new NotImplementedException();
            }
        }
    }
}
