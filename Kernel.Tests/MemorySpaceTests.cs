using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Kernel;

namespace Kernel.Tests
{
    [TestFixture]
    public class MemorySpaceTests
    {
        [Test]
        public void ConstructorTests()
        {
            var m = new MemorySpace(0, 100);
            Assert.AreEqual(100, m.Size,"#001");
            Assert.AreEqual(0, m.StartAddress, "#002");
            Assert.AreEqual(99, m.EndAddress, "#003");
            Assert.IsFalse(m.Free, "#004");
            Assert.IsNull(m.Next, "#005");
            Assert.IsNull(m.Previous, "#006");
            

            m = new MemorySpace(true, 0, 99);
            Assert.AreEqual(100, m.Size, "#007");
            Assert.AreEqual(0, m.StartAddress, "#008");
            Assert.AreEqual(99, m.EndAddress, "#009");
            Assert.IsTrue(m.Free, "#010");
            Assert.IsNull(m.Next, "#011");
            Assert.IsNull(m.Previous, "#012");

            m = new MemorySpace(0, 100, false);
            Assert.AreEqual(100, m.Size, "#013");
            Assert.AreEqual(0, m.StartAddress, "#014");
            Assert.AreEqual(99, m.EndAddress, "#015");
            Assert.IsFalse(m.Free, "#016");
            Assert.IsNull(m.Next, "#017");
            Assert.IsNull(m.Previous, "#018");

            
            var prev = new MemorySpace(0, 0);
            var next = new MemorySpace(0, 0);
            m = new MemorySpace(false, 0, 99, prev, next);
            Assert.AreEqual(100, m.Size, "#019");
            Assert.AreEqual(0, m.StartAddress, "#020");
            Assert.AreEqual(99, m.EndAddress, "#021");
            Assert.IsFalse(m.Free, "#022");
            Assert.AreEqual(next, m.Next, "#023");
            Assert.AreEqual(prev, m.Previous, "#024");
            Assert.AreEqual(m, next.Previous, "#025");
            Assert.AreEqual(m, prev.Next, "#026");

            prev = new MemorySpace(0, 0);
            next = new MemorySpace(0, 0);
            m = new MemorySpace(0, 100,false, prev, next);
            Assert.AreEqual(100, m.Size, "#027");
            Assert.AreEqual(0, m.StartAddress, "#028");
            Assert.AreEqual(99, m.EndAddress, "#029");
            Assert.IsFalse(m.Free, "#030");
            Assert.AreEqual(next, m.Next, "#031");
            Assert.AreEqual(prev, m.Previous, "#032");
            Assert.AreEqual(m, next.Previous, "#033");
            Assert.AreEqual(m, prev.Next, "#034");

        }

        [Test]
        public void SizeChange()
        {
            var m = new MemorySpace(0, 100);
            Assert.AreEqual(100, m.Size, "#001");
            Assert.AreEqual(0, m.StartAddress, "#002");
            Assert.AreEqual(99, m.EndAddress, "#003");
            Assert.IsFalse(m.Free, "#004");
            Assert.IsNull(m.Next, "#005");
            Assert.IsNull(m.Previous, "#006");

            m.Size = 30;

            Assert.AreEqual(30, m.Size, "#007");
            Assert.AreEqual(0, m.StartAddress, "#008");
            Assert.AreEqual(29, m.EndAddress, "#009");
            Assert.IsFalse(m.Free, "#010");
            Assert.IsNull(m.Next, "#011");
            Assert.IsNull(m.Previous, "#012");
        }
    }
}
