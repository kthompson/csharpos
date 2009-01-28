using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Indy.IL2CPU
{
    public class AbstractStructureVisitor : AbstractVisitor<AbstractStructureVisitor>, IReflectionStructureVisitor
    {


        #region IReflectionStructureVisitor Members
        public Action<AbstractStructureVisitor, AssemblyDefinition> TerminateAssemblyDefinition;
        void IReflectionStructureVisitor.TerminateAssemblyDefinition(AssemblyDefinition asm)
        {
            Run(TerminateAssemblyDefinition, asm);
        }

        public Action<AbstractStructureVisitor, AssemblyDefinition> VisitAssemblyDefinition;
        void IReflectionStructureVisitor.VisitAssemblyDefinition(AssemblyDefinition asm)
        {
            Run(VisitAssemblyDefinition, asm);
        }

        public Action<AbstractStructureVisitor, AssemblyLinkedResource> VisitAssemblyLinkedResource;
        void IReflectionStructureVisitor.VisitAssemblyLinkedResource(AssemblyLinkedResource res)
        {
            Run(VisitAssemblyLinkedResource, res);
        }

        public Action<AbstractStructureVisitor, AssemblyNameDefinition> VisitAssemblyNameDefinition;
        void IReflectionStructureVisitor.VisitAssemblyNameDefinition(AssemblyNameDefinition name)
        {
            Run(VisitAssemblyNameDefinition, name);
        }

        public Action<AbstractStructureVisitor, AssemblyNameReference> VisitAssemblyNameReference;
        void IReflectionStructureVisitor.VisitAssemblyNameReference(AssemblyNameReference name)
        {
            Run(VisitAssemblyNameReference, name);
        }

        void IReflectionStructureVisitor.VisitAssemblyNameReferenceCollection(AssemblyNameReferenceCollection names)
        {
            Run(VisitAssemblyNameReference, names);
        }

        public Action<AbstractStructureVisitor, EmbeddedResource> VisitEmbeddedResource;
        void IReflectionStructureVisitor.VisitEmbeddedResource(EmbeddedResource res)
        {
            Run(VisitEmbeddedResource, res);
        }

        public Action<AbstractStructureVisitor, LinkedResource> VisitLinkedResource;
        void IReflectionStructureVisitor.VisitLinkedResource(LinkedResource res)
        {
            Run(VisitLinkedResource, res);
        }

        public Action<AbstractStructureVisitor, ModuleDefinition> VisitModuleDefinition;
        void IReflectionStructureVisitor.VisitModuleDefinition(ModuleDefinition module)
        {
            Run(VisitModuleDefinition, module);
        }

        void IReflectionStructureVisitor.VisitModuleDefinitionCollection(ModuleDefinitionCollection modules)
        {
            Run(VisitModuleDefinition, modules);
        }

        public Action<AbstractStructureVisitor, ModuleReference> VisitModuleReference;
        void IReflectionStructureVisitor.VisitModuleReference(ModuleReference module)
        {
            Run(VisitModuleReference, module);
        }

        void IReflectionStructureVisitor.VisitModuleReferenceCollection(ModuleReferenceCollection modules)
        {
            Run(VisitModuleReference, modules);
        }

        public Action<AbstractStructureVisitor, ResourceCollection> VisitResourceCollection;
        void IReflectionStructureVisitor.VisitResourceCollection(ResourceCollection resources)
        {
            Run(VisitResourceCollection, resources);
        }

        #endregion
    }
}
