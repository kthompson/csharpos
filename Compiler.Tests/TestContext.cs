using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Framework;
using Mono.Cecil;

namespace Compiler.Tests
{
    class TestContext : AssemblyCompilerContext
    {
        public object[] Arguments { get; private set; }

        public TestContext(AssemblyDefinition assembly, object[] arguments)
            : base(assembly, "test.exe")
        {
            this.Arguments = arguments;
        }
    }
}
