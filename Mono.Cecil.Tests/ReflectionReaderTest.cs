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
* Temporary test, should be injected with AspectDNG
*
*****************************************************************************/

namespace Mono.Cecil.Tests {

    using System;

    using Mono.Cecil;
    using Mono.Cecil.Binary;
    using Mono.Cecil.Metadata;

    using NUnit.Framework;

    [TestFixture]
    public sealed class ReflectionReaderTest {

        private IAssemblyDefinition m_asmdef;

        [SetUp]
        public void SetUp ()
        {
            if (m_asmdef == null)
                m_asmdef = AssemblyFactory.GetAssembly (@"D:\System.Web.dll");
        }

        [Test]
        public void PourVoir ()
        {
            Console.WriteLine (m_asmdef.Name.FullName);
            Console.WriteLine ("modules : {0}", m_asmdef.Modules.Count);
            foreach (IModuleDefinition def in m_asmdef.Modules) {
                Console.WriteLine ("module name : {0}", def.Name);
                Console.WriteLine ("module guid : {0}", def.Mvid.ToString ());

                Console.WriteLine ("asm refs : {0}", def.AssemblyReferences.Count);
                foreach (IAssemblyNameReference name in def.AssemblyReferences) {
                    Console.WriteLine ("asm ref : {0}", name.FullName);
                }

                Console.WriteLine ("module ref : {0}", def.ModuleReferences.Count);
                foreach (IModuleReference mr in def.ModuleReferences) {
                    Console.WriteLine ("module ref : {0}", mr.Name);
                }

                /*foreach (ITypeDefinition type in def.Types) {
                    Console.WriteLine (type.FullName);
                }*/

                if (def.Main) {
                    ITypeDefinition ctrl = def.Types ["System.Web.UI.Control"];
                    Console.WriteLine ("Control base type: " + ctrl.BaseType.FullName);

                    Console.WriteLine ("Control implements :");
                    foreach (ITypeReference interf in ctrl.Interfaces) {
                        Console.WriteLine (interf.FullName);
                    }

                    foreach (IFieldDefinition field in ctrl.Fields) {
                        Console.WriteLine ("field: {0} {1}", field.FieldType.FullName, field.Name);
                    }

                    foreach (IMethodDefinition meth in ctrl.Methods) {
                        Console.Write ("method: ");
                        Console.Write (meth.ReturnType.ReturnType.FullName);
                        Console.Write (" ");
                        Console.Write (meth.DeclaringType.FullName);
                        Console.Write ("::");
                        Console.Write (meth.Name);
                        Console.Write ("(");
                        for (int i = 0; i < meth.Parameters.Count; i++) {
                            IParameterDefinition param = meth.Parameters [i];
                            Console.Write (param.ParameterType.FullName);
                            Console.Write (" ");
                            Console.Write (param.Name);
                            if (i < meth.Parameters.Count - 1)
                                Console.Write (", ");
                        }
                        Console.WriteLine (")");
                    }

                    foreach (IPropertyDefinition prop in ctrl.Properties) {
                        Console.WriteLine ("property: " + prop.Name);
                    }

                    foreach (IEventDefinition evt in ctrl.Events) {
                        Console.WriteLine ("event: " + evt.Name);
                    }
                }
            }
        }
    }
}
