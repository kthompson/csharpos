using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Xunit;

namespace Kernel.Tests
{
    public class HeapTests
    {
        [Fact]
        public void AllocationTest()
        {
           var heap = Heap.Instance;

            Assert.NotNull(heap);
            var m1 = Heap.Instance.AllocateMemory(10);
            Assert.Equal(0u, m1.StartAddress);
            Assert.Equal(10u, m1.Size);
            Assert.Equal(9u, m1.EndAddress);
            Assert.False(m1.Free);

            var m2 = Heap.Instance.AllocateMemory(10);
            Assert.Equal(10u, m2.StartAddress);
            Assert.Equal(10u, m2.Size);
            Assert.Equal(19u, m2.EndAddress);
            Assert.False(m2.Free);

            var m3 = Heap.Instance.AllocateMemory(10);
            Assert.Equal(20u, m3.StartAddress);
            Assert.Equal(10u, m3.Size);
            Assert.Equal(29u, m3.EndAddress);
            Assert.False(m3.Free);

            Heap.Instance.FreeMemory(m2);

            Assert.True(m2.Free);

            var m4 = Heap.Instance.AllocateMemory(5);
            Assert.Equal(10u, m4.StartAddress);
            Assert.Equal(5u, m4.Size);
            Assert.Equal(14u, m4.EndAddress);
            Assert.False(m3.Free);
        }
    }
}
