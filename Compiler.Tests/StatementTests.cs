using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;

namespace Compiler.Tests
{
    [TestFixture]
    public class StatementTests : CompilerTest
    {
        [Test]
        public void IfStatementTests()
        {
            Assert.AreEqual("5", CompileAndRunMethod(() =>
            {
                var field1 = true;
                if (field1)
                    return 5;

                return 0;
            }));

            Assert.AreEqual("5", CompileAndRunMethod(() =>
            {
                var field1 = true;
                var field2 = false;
                if (field1)
                    if (field2)
                        return 2;
                    else
                        return 5;
                return 0;
            }));
        }
    }
}
