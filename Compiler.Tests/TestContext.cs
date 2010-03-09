using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Compiler.Tests
{
    class TestContext : AssemblyCompilerContext
    {
        public object[] Arguments { get; private set; }

        public TestContext(AssemblyDefinition assembly, MethodDefinition method, object[] arguments)
            : base(assembly, method)
        {
            this.Arguments = arguments;
        }
    }
}
