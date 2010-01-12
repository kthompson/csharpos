using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Translator;
using System.IO;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Compiler.Tests
{
    [TestFixture]
    public class Phase1Tests : CompilerTest
    {
        [Test]
        public void IntegerTests()
        {
            var asm = AssemblyFactory.GetAssembly("Phase1Tests.Patched.dll");
            var main = asm.MainModule;
            var type = main.Types["Phase1Tests.Phase1Tests"];
            var output = CompileAndRunMethod(type, "TestMethod1");
            Assert.AreEqual("7\r\n", output);
        }
    }
}
