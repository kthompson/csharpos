using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.Reflection;

namespace Kernel.Tests
{
    [TestFixture]
    public class HeapTests
    {
        [Test]
        public void AllocationTest()
        {
           var heap = Heap.Instance;

            Assert.IsNotNull(heap, "#01");
            var m1 = Heap.Instance.AllocateMemory(10);
            Assert.AreEqual(0u, m1.StartAddress, "#02");
            Assert.AreEqual(10u, m1.Size, "#03");
            Assert.AreEqual(9u, m1.EndAddress, "#04");
            Assert.IsFalse(m1.Free, "#05");

            var m2 = Heap.Instance.AllocateMemory(10);
            Assert.AreEqual(10u, m2.StartAddress, "#06");
            Assert.AreEqual(10u, m2.Size, "#07");
            Assert.AreEqual(19u, m2.EndAddress, "#08");
            Assert.IsFalse(m2.Free, "#09");

            var m3 = Heap.Instance.AllocateMemory(10);
            Assert.AreEqual(20u, m3.StartAddress, "#10");
            Assert.AreEqual(10u, m3.Size, "#11");
            Assert.AreEqual(29u, m3.EndAddress, "#12");
            Assert.IsFalse(m3.Free, "#13");

            Heap.Instance.FreeMemory(m2);

            Assert.IsTrue(m2.Free, "#14");

            var m4 = Heap.Instance.AllocateMemory(5);
            Assert.AreEqual(10u, m4.StartAddress, "#15");
            Assert.AreEqual(5u, m4.Size, "#16");
            Assert.AreEqual(14u, m4.EndAddress, "#17");
            Assert.IsFalse(m3.Free, "#18");
            

        }
    }
}
