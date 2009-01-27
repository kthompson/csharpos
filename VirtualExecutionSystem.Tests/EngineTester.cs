using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace VirtualExecutionSystem.Tests
{
    [TestFixture]   
    public class EngineTester
    {
        [Test]
        public void Debugger()
        {
            var engine = new Engine(typeof(EngineTester).GetMethod("DebuggerTest"));
            engine.Start();
        }

        public void DebuggerTest()
        {
            var i = 1;
            var j = 2 + i;
            var k = 3 + j;
            var l = 4 + k;
            var m = 5 + l;
        }
    }
}
