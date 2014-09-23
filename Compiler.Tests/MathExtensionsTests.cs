using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Compiler.Tests
{
    public class MathExtensionsTests
    {
        [Fact]
        public void ToIEEE754Tests()
        {
            unchecked
            {
                Assert.Equal(1078523331, (3.14f).ToIEEE754());
                Assert.Equal(0x411c0000, (9.75f).ToIEEE754());
                Assert.Equal(0x3da00000, (0.078125f).ToIEEE754());
                Assert.Equal((int)0xc2c08a3d, (-96.27f).ToIEEE754());    
            }
        }
    }
}
