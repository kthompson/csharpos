using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.IO;
using MbUnit.Framework;
using Assert = MbUnit.Framework.Assert;

namespace Compiler.Tests
{
    [TestFixture]
    public class ImmediateTests : CompilerTest
    {
        [Test]
        public void Integers()
        {
            Assert.AreEqual("0", CompileAndRunMethod(() => 0));
            Assert.AreEqual("1", CompileAndRunMethod(() => 1));
            Assert.AreEqual("-1", CompileAndRunMethod(() => -1));
            Assert.AreEqual("10", CompileAndRunMethod(() => 10));
            Assert.AreEqual("-10", CompileAndRunMethod(() => -10));
            Assert.AreEqual("2736", CompileAndRunMethod(() => 2736));
            Assert.AreEqual("-2736", CompileAndRunMethod(() => -2736));
            Assert.AreEqual("536870911", CompileAndRunMethod(() => 536870911));
            Assert.AreEqual("-536870912", CompileAndRunMethod(() => -536870912));
        }

        [Test]
        public void UnsignedIntegers()
        {
            Assert.AreEqual("0", CompileAndRunMethod(() => 0U));
            Assert.AreEqual("1", CompileAndRunMethod(() => 1U));
            Assert.AreEqual("10", CompileAndRunMethod(() => 10U));
            Assert.AreEqual("2736", CompileAndRunMethod(() => 2736U));
            Assert.AreEqual("536870911", CompileAndRunMethod(() => 536870911U));
        }

        [Test]
        public void Longs()
        {
            Assert.AreEqual("-5368709121234", CompileAndRunMethod(() => -5368709121234L));
            Assert.AreEqual("429496121113456735", CompileAndRunMethod(() => 429496121113456735L));
        }

        [Test]
        public void UnsignedLongs()
        {
            Assert.AreEqual("5368709121234", CompileAndRunMethod(() => 5368709121234LU));
            Assert.AreEqual("429496121113456735", CompileAndRunMethod(() => 429496121113456735LU));
        }

        [Test]
        public void Boolean()
        {
            Assert.AreEqual("True", CompileAndRunMethod(() => true));
            Assert.AreEqual("False", CompileAndRunMethod(() => false));
        }


        [Test]
        public void Characters()
        {
            Assert.AreEqual("a", CompileAndRunMethod(() => 'a'));
            Assert.AreEqual("b", CompileAndRunMethod(() => 'b'));
            Assert.AreEqual("c", CompileAndRunMethod(() => 'c'));
            Assert.AreEqual("d", CompileAndRunMethod(() => 'd'));
            Assert.AreEqual("e", CompileAndRunMethod(() => 'e'));
            Assert.AreEqual("f", CompileAndRunMethod(() => 'f'));

            Assert.AreEqual("0", CompileAndRunMethod(() => '0'));
            Assert.AreEqual("1", CompileAndRunMethod(() => '1'));
            Assert.AreEqual("2", CompileAndRunMethod(() => '2'));
            Assert.AreEqual("3", CompileAndRunMethod(() => '3'));
            Assert.AreEqual("4", CompileAndRunMethod(() => '4'));
            Assert.AreEqual("5", CompileAndRunMethod(() => '5'));
        }

        [Test]
        public void Floats()
        {
            Assert.AreEqual("3.140", CompileAndRunMethod(() => 3.14f));
            Assert.AreEqual("-3.140", CompileAndRunMethod(() => -3.14f));
            Assert.AreEqual("-9.750", CompileAndRunMethod(() => -9.75f));
            Assert.AreEqual("9.750", CompileAndRunMethod(() => 9.75f));
            Assert.AreEqual("0.141", CompileAndRunMethod(() => 0.141234f));
            Assert.AreEqual("1233.114", CompileAndRunMethod(() => 1233.114f));
        }

        [Test]
        public void Doubles()
        {
            Assert.AreEqual("3.140", CompileAndRunMethod(() => 3.14d));
            Assert.AreEqual("-3.140", CompileAndRunMethod(() => -3.14d));
            Assert.AreEqual("-9.750", CompileAndRunMethod(() => -9.75d));
            Assert.AreEqual("9.750", CompileAndRunMethod(() => 9.75d));
            Assert.AreEqual("0.141", CompileAndRunMethod(() => 0.141234d));
            Assert.AreEqual("1233.114", CompileAndRunMethod(() => 1233.114d));
        }

    }
}
