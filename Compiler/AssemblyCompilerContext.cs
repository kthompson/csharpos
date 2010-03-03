using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler
{
    public class AssemblyCompilerContext : ICompilerContext
    {
        public AssemblyDefinition AssemblyDefinition { get; private set; }

        public AssemblyCompilerContext(AssemblyDefinition assemblyDefinition)
        {
            this.AssemblyDefinition = assemblyDefinition;
        }
    }
}
