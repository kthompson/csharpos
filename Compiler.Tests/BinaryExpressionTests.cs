using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;

namespace Compiler.Tests
{
    [TestFixture]
    public class BinaryExpressionTests : CompilerTest
    {
        [Test]
        public void Addition()
        {
            Assert.AreEqual("3", CompileAndRunMethod(() =>
            {
                var field1 = 1;
                var field2 = 2;
                return field1 + field2;
            }));

            Assert.AreEqual("3", CompileAndRunMethod(() =>
            {
                var field1 = 1;
                var field2 = 2;
                var field3 = field1 + field2;
                return field3;
            }));

            Assert.AreEqual("1", CompileAndRunMethod(() =>
            {
                var field1 = -1;
                var field2 = 2;
                var field3 = field1 + field2;
                return field3;
            }));

            Assert.AreEqual("1", CompileAndRunMethod(() =>
            {
                var field1 = -1;
                var field2 = 2;
                var field3 = field1 + field2;
                return field3;
            }));
        }

        [Test]
        public void Subtraction()
        {
            Assert.AreEqual("-1", CompileAndRunMethod(() =>
            {
                var field1 = 1;
                var field2 = 2;
                return field1 - field2;
            }));

            Assert.AreEqual("-1", CompileAndRunMethod(() =>
            {
                var field1 = 1;
                var field2 = 2;
                var field3 = field1 - field2;
                return field3;
            }));

            Assert.AreEqual("-3", CompileAndRunMethod(() =>
            {
                var field1 = -1;
                var field2 = 2;
                var field3 = field1 - field2;
                return field3;
            }));

            Assert.AreEqual("-3", CompileAndRunMethod(() =>
            {
                var field1 = -1;
                var field2 = 2;
                var field3 = field1 - field2;
                return field3;
            }));
        }

        [Test]
        public void Division()
        {
            Assert.AreEqual("3", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 / field2;
            }));

            Assert.AreEqual("32", CompileAndRunMethod(() =>
            {
                var field1 = 256;
                var field2 = 8;
                return field1 / field2;
            }));

            Assert.AreEqual("-32", CompileAndRunMethod(() =>
            {
                var field1 = -256;
                var field2 = 8;
                return field1 / field2;
            }));

            Assert.AreEqual("32", CompileAndRunMethod(() =>
            {
                var field1 = -256;
                var field2 = -8;
                return field1 / field2;
            }));

            Assert.AreEqual("3", CompileAndRunMethod(() =>
            {
                var field1 = 25;
                var field2 = 8;
                return field1 / field2;
            }));

            Assert.AreEqual("3", CompileAndRunMethod(() =>
            {
                var field1 = 26;
                var field2 = 8;
                return field1 / field2;
            }));

            Assert.AreEqual("3", CompileAndRunMethod(() =>
            {
                var field1 = 27;
                var field2 = 8;
                return field1 / field2;
            }));

            Assert.AreEqual("3", CompileAndRunMethod(() =>
            {
                var field1 = 30;
                var field2 = 8;
                return field1 / field2;
            }));
        }

        [Test]
        public void Multiplication()
        {
            Assert.AreEqual("12", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 * field2;
            }));

            Assert.AreEqual("256", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 * field2;
            }));

            Assert.AreEqual("-256", CompileAndRunMethod(() =>
            {
                var field1 = -32;
                var field2 = 8;
                return field1 * field2;
            }));

            Assert.AreEqual("-256", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = -8;
                return field1 * field2;
            }));

            Assert.AreEqual("24", CompileAndRunMethod(() =>
            {
                var field1 = 3;
                var field2 = 8;
                return field1 * field2;
            }));
        }

        [Test]
        public void BitwiseAnd()
        {
            Assert.AreEqual("2", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 & field2;
            }));

            Assert.AreEqual("0", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 & field2;
            }));

            Assert.AreEqual("0", CompileAndRunMethod(() =>
            {
                var field1 = -32;
                var field2 = 8;
                return field1 & field2;
            }));

            Assert.AreEqual("8", CompileAndRunMethod(() =>
            {
                var field1 = 25;
                var field2 = 8;
                return field1 & field2;
            }));

            Assert.AreEqual("1", CompileAndRunMethod(() =>
            {
                var field1 = 23;
                var field2 = 9;
                return field1 & field2;
            }));
        }

        [Test]
        public void BitwiseOr()
        {
            Assert.AreEqual("6", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 | field2;
            }));

            Assert.AreEqual("40", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 | field2;
            }));

            Assert.AreEqual("264", CompileAndRunMethod(() =>
            {
                var field1 = 256;
                var field2 = 8;
                return field1 | field2;
            }));

            Assert.AreEqual("57", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 25;
                return field1 | field2;
            }));

            Assert.AreEqual("11", CompileAndRunMethod(() =>
            {
                var field1 = 3;
                var field2 = 8;
                return field1 | field2;
            }));
        }

        [Test]
        public void BitwiseXor()
        {
            Assert.AreEqual("4", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 ^ field2;
            }));

            Assert.AreEqual("40", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 ^ field2;
            }));

            Assert.AreEqual("17", CompileAndRunMethod(() =>
            {
                var field1 = 25;
                var field2 = 8;
                return field1 ^ field2;
            }));

            Assert.AreEqual("177", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 145;
                return field1 ^ field2;
            }));

            Assert.AreEqual("26", CompileAndRunMethod(() =>
            {
                var field1 = 123;
                var field2 = 97;
                return field1 ^ field2;
            }));
        }

        [Test]
        public void EqualityTests()
        {
            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 == field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 == field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 == field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = true;
                var field2 = false;
                return field1 == field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 == field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 == field2;
            }));
        }

        [Test]
        public void InequalityTests()
        {
            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 6;
                var field2 = 2;
                return field1 != field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 != field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 != field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = true;
                var field2 = false;
                return field1 != field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 != field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 != field2;
            }));
        }

        [Test]
        public void LessThan()
        {
            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = int.MinValue;
                var field2 = int.MaxValue;
                return field1 < field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = uint.MinValue;
                var field2 = uint.MaxValue;
                return field1 < field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 2;
                var field2 = 6;
                return field1 < field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 < field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = -32;
                var field2 = 8;
                return field1 < field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = -123;
                var field2 = 2343;
                return field1 < field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 < field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 < field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 < field2;
            }));
        }

        [Test]
        public void LessThanOrEqual()
        {
            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = int.MinValue;
                var field2 = int.MaxValue;
                return field1 <= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = uint.MinValue;
                var field2 = uint.MaxValue;
                return field1 <= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 2;
                var field2 = 6;
                return field1 <= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = -2;
                var field2 = 6;
                return field1 <= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = -32;
                var field2 = 32;
                return field1 <= field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 <= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 <= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 <= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 <= field2;
            }));
        }

        [Test]
        public void GreaterThan()
        {

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = int.MaxValue;
                var field2 = int.MinValue;
                return field1 > field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = uint.MaxValue;
                var field2 = uint.MinValue;
                return field1 > field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 2;
                var field2 = 6;
                return field1 > field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 > field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = -32;
                var field2 = 8;
                return field1 > field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = -8;
                var field2 = -32;
                return field1 > field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 > field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 > field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 > field2;
            }));
        }

        [Test]
        public void GreaterThanOrEqual()
        {
            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 2;
                var field2 = 6;
                return field1 >= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = 8;
                return field1 >= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = int.MaxValue;
                var field2 = int.MinValue;
                return field1 >= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = uint.MaxValue;
                var field2 = uint.MinValue;
                return field1 >= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = -12;
                var field2 = -12;
                return field1 >= field2;
            }));


            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = -122;
                var field2 = -128;
                return field1 >= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 32;
                var field2 = -8;
                return field1 >= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 8;
                var field2 = 8;
                return field1 >= field2;
            }));

            Assert.AreEqual("False", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'b';
                return field1 >= field2;
            }));

            Assert.AreEqual("True", CompileAndRunMethod(() =>
            {
                var field1 = 'a';
                var field2 = 'a';
                return field1 >= field2;
            }));
        }
    }
}
