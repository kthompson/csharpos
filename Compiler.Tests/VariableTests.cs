using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Compiler.Tests
{
    public class VariableTests : CompilerTest
    {
        [Fact]
        public void VariableReferenceTests()
        {
            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var fieldT = true;
                return fieldT;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var fieldF = false;
                return fieldF;
            }));

            Assert.Equal("1", CompileAndRunMethod(() =>
            {
                var fieldF = 1;
                return fieldF;
            }));

            Assert.Equal("2", CompileAndRunMethod(() =>
            {
                var fieldF = 2;
                return fieldF;
            }));

            Assert.Equal("a", CompileAndRunMethod(() =>
            {
                var fieldF = 'a';
                return fieldF;
            }));

            Assert.Equal("5", CompileAndRunMethod(() =>
            {
                var fieldF = 5;
                var field2 = fieldF;
                return field2;
            }));

            Assert.Equal("a", CompileAndRunMethod(() =>
            {
                var fieldF = 'a';
                var field2 = fieldF;
                return field2;
            }));

            Assert.Equal("-2", CompileAndRunMethod(() =>
            {
                var fieldF = -2;
                return fieldF;
            }));
        }
    }
}
