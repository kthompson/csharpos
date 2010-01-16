using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Translator;

namespace Compiler.Tests
{
    [TestFixture]
    public class MathExtensionsTests
    {
        [Test]
        public void ToIEEE754Tests()
        {
            unchecked
            {
                Assert.AreEqual(1078523331, (3.14f).ToIEEE754());
                Assert.AreEqual(0x411c0000, (9.75f).ToIEEE754());
                Assert.AreEqual(0x3da00000, (0.078125f).ToIEEE754());
                Assert.AreEqual((int)0xc2c08a3d, (-96.27f).ToIEEE754());    
            }
        }
    }
}
