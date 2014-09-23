using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

// We dont want to apply these inspections since they would change the tests
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable ConvertToConstant.Local

namespace Compiler.Tests
{
    public class BinaryExpressionTests : CompilerTest
    {
        public void AssertBinaryExpression<T>(T a, T b, T c, Func<T, T, T> action)
        {
            Assert.Equal(c.ToString(), CompileAndRunMethod(action, a, b));
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(-1, 2, 1)]
        [InlineData(-45, 2, -43)]
        public void Addition(int a, int b, int c)
        {
            AssertBinaryExpression(a, b, c, (aa, bb) => aa + bb);
        }

        [Fact]
        [InlineData(1, 2, -1)]
        [InlineData(-1, 2, -3)]
        [InlineData(0, 2, -2)]
        [InlineData(0, 0, 0)]
        [InlineData(-45, 2, -47)]
        public void Subtraction(int a, int b, int c)
        {
            AssertBinaryExpression(a, b, c, (aa, bb) => aa - bb);
        }


        [Fact]
        public void Division()
        {
            Assert.Equal("3", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 / field2;
            }));

            Assert.Equal("32", CompileAndRunMethod(() =>
            {
                var field1 = 256;
                var field2 = 8;
                return field1 / field2;
            }));

            Assert.Equal("-32", CompileAndRunMethod(() =>
            {
                var field1 = -256;
                var field2 = 8;
                return field1 / field2;
            }));

            Assert.Equal("32", CompileAndRunMethod(() =>
            {
                var field1 = -256;
                var field2 = -8;
                return field1 / field2;
            }));

            Assert.Equal("3", CompileAndRunMethod(() =>
            {
                var field1 = 25;
                var field2 = 8;
                return field1 / field2;
            }));

            Assert.Equal("3", CompileAndRunMethod(() =>
            {
                var field1 = 26;
                var field2 = 8;
                return field1 / field2;
            }));

            Assert.Equal("3", CompileAndRunMethod(() =>
            {
                var field1 = 27;
                var field2 = 8;
                return field1 / field2;
            }));

            Assert.Equal("3", CompileAndRunMethod(() =>
            {
                var field1 = 30;
                var field2 = 8;
                return field1 / field2;
            }));
        }

        [Fact]
        public void Multiplication()
        {
            Assert.Equal("12", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 * field2;
            }));

            Assert.Equal("256", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 * field2;
            }));

            Assert.Equal("-256", CompileAndRunMethod(() =>
            {
                var field1 = -32;
                var field2 = 8;
                return field1 * field2;
            }));

            Assert.Equal("-256", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = -8;
                return field1 * field2;
            }));

            Assert.Equal("24", CompileAndRunMethod(() =>
            {
                var field1 = 3;
                var field2 = 8;
                return field1 * field2;
            }));
        }

        [Fact]
        public void BitwiseAnd()
        {
            Assert.Equal("2", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 & field2;
            }));

            Assert.Equal("0", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 & field2;
            }));

            Assert.Equal("0", CompileAndRunMethod(() =>
            {
                var field1 = -32;
                var field2 = 8;
                return field1 & field2;
            }));

            Assert.Equal("8", CompileAndRunMethod(() =>
            {
                var field1 = 25;
                var field2 = 8;
                return field1 & field2;
            }));

            Assert.Equal("1", CompileAndRunMethod(() =>
            {
                var field1 = 23;
                var field2 = 9;
                return field1 & field2;
            }));
        }

        [Fact]
        public void BitwiseOr()
        {
            Assert.Equal("6", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 | field2;
            }));

            Assert.Equal("40", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 | field2;
            }));

            Assert.Equal("264", CompileAndRunMethod(() =>
            {
                var field1 = 256;
                var field2 = 8;
                return field1 | field2;
            }));

            Assert.Equal("57", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 25;
                return field1 | field2;
            }));

            Assert.Equal("11", CompileAndRunMethod(() =>
            {
                var field1 = 3;
                var field2 = 8;
                return field1 | field2;
            }));
        }

        [Fact]
        public void BitwiseXor()
        {
            Assert.Equal("4", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 ^ field2;
            }));

            Assert.Equal("40", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 ^ field2;
            }));

            Assert.Equal("17", CompileAndRunMethod(() =>
            {
                var field1 = 25;
                var field2 = 8;
                return field1 ^ field2;
            }));

            Assert.Equal("177", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 145;
                return field1 ^ field2;
            }));

            Assert.Equal("26", CompileAndRunMethod(() =>
            {
                var field1 = 123;
                var field2 = 97;
                return field1 ^ field2;
            }));
        }

        [Fact]
        public void EqualityTests()
        {
            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 == field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 == field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 == field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = true;
                var field2 = false;
                return field1 == field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 == field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 == field2;
            }));
        }

        [Fact]
        public void InequalityTests()
        {
            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 != field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 != field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 != field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = true;
                var field2 = false;
                return field1 != field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 != field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 != field2;
            }));
        }

        [Fact]
        public void LessThan()
        {
            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = int.MinValue;
                var field2 = int.MaxValue;
                return field1 < field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = uint.MinValue;
                var field2 = uint.MaxValue;
                return field1 < field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 2;
                var field2 = 6;
                return field1 < field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 < field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = -32;
                var field2 = 8;
                return field1 < field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = -123;
                var field2 = 2343;
                return field1 < field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 < field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 < field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 < field2;
            }));
        }

        [Fact]
        public void LessThanOrEqual()
        {
            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = int.MinValue;
                var field2 = int.MaxValue;
                return field1 <= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = uint.MinValue;
                var field2 = uint.MaxValue;
                return field1 <= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 2;
                var field2 = 6;
                return field1 <= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = -2;
                var field2 = 6;
                return field1 <= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = -32;
                var field2 = 32;
                return field1 <= field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 <= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 <= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 <= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 <= field2;
            }));
        }

        [Fact]
        public void GreaterThan()
        {

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = int.MaxValue;
                var field2 = int.MinValue;
                return field1 > field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = uint.MaxValue;
                var field2 = uint.MinValue;
                return field1 > field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 2;
                var field2 = 6;
                return field1 > field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 > field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = -32;
                var field2 = 8;
                return field1 > field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = -8;
                var field2 = -32;
                return field1 > field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 > field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 > field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 > field2;
            }));
        }

        [Fact]
        public void GreaterThanOrEqual()
        {
            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 2;
                var field2 = 6;
                return field1 >= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 >= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = int.MaxValue;
                var field2 = int.MinValue;
                return field1 >= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = uint.MaxValue;
                var field2 = uint.MinValue;
                return field1 >= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = -12;
                var field2 = -12;
                return field1 >= field2;
            }));


            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = -122;
                var field2 = -128;
                return field1 >= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = -8;
                return field1 >= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 >= field2;
            }));

            Assert.Equal("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 >= field2;
            }));

            Assert.Equal("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 >= field2;
            }));
        }
    }
}
