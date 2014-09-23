using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.IO;
using Xunit;

namespace Compiler.Tests
{
    public class ImmediateTests : CompilerTest
    {
        [Fact]
        public void Integers()
        {
            Assert.Equal("0", CompileAndRunMethod(() => 0));
            Assert.Equal("1", CompileAndRunMethod(() => 1));
            Assert.Equal("-1", CompileAndRunMethod(() => -1));
            Assert.Equal("10", CompileAndRunMethod(() => 10));
            Assert.Equal("-10", CompileAndRunMethod(() => -10));
            Assert.Equal("2736", CompileAndRunMethod(() => 2736));
            Assert.Equal("-2736", CompileAndRunMethod(() => -2736));
            Assert.Equal("536870911", CompileAndRunMethod(() => 536870911));
            Assert.Equal("-536870912", CompileAndRunMethod(() => -536870912));
        }

        [Fact]
        public void Longs()
        {
            Assert.Equal("-5368709121234", CompileAndRunMethod(() => -5368709121234L));
            Assert.Equal("429496121113456735", CompileAndRunMethod(() => 429496121113456735L));
        }

        [Fact]
        public void Boolean()
        {
            Assert.Equal("True", CompileAndRunMethod(() => true));
            Assert.Equal("False", CompileAndRunMethod(() => false));
        }


        [Fact]
        public void Characters()
        {
            Assert.Equal("a", CompileAndRunMethod(() => 'a'));
            Assert.Equal("b", CompileAndRunMethod(() => 'b'));
            Assert.Equal("c", CompileAndRunMethod(() => 'c'));
            Assert.Equal("d", CompileAndRunMethod(() => 'd'));
            Assert.Equal("e", CompileAndRunMethod(() => 'e'));
            Assert.Equal("f", CompileAndRunMethod(() => 'f'));

            Assert.Equal("0", CompileAndRunMethod(() => '0'));
            Assert.Equal("1", CompileAndRunMethod(() => '1'));
            Assert.Equal("2", CompileAndRunMethod(() => '2'));
            Assert.Equal("3", CompileAndRunMethod(() => '3'));
            Assert.Equal("4", CompileAndRunMethod(() => '4'));
            Assert.Equal("5", CompileAndRunMethod(() => '5'));
        }

        [Fact]
        public void Floats()
        {
            Assert.Equal("3.140", CompileAndRunMethod(() => 3.14f));
            Assert.Equal("-3.140", CompileAndRunMethod(() => -3.14f));
            Assert.Equal("-9.750", CompileAndRunMethod(() => -9.75f));
            Assert.Equal("9.750", CompileAndRunMethod(() => 9.75f));
            Assert.Equal("0.141", CompileAndRunMethod(() => 0.141234f));
            Assert.Equal("1233.114", CompileAndRunMethod(() => 1233.114f));
        }

        [Fact]
        public void Doubles()
        {
            Assert.Equal("3.140", CompileAndRunMethod(() => 3.14d));
            Assert.Equal("-3.140", CompileAndRunMethod(() => -3.14d));
            Assert.Equal("-9.750", CompileAndRunMethod(() => -9.75d));
            Assert.Equal("9.750", CompileAndRunMethod(() => 9.75d));
            Assert.Equal("0.141", CompileAndRunMethod(() => 0.141234d));
            Assert.Equal("1233.114", CompileAndRunMethod(() => 1233.114d));
        }

    }
}
