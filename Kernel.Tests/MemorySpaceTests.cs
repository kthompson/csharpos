using System;
using System.Collections.Generic;
using System.Text;
using Kernel;
using Xunit;

namespace Kernel.Tests
{
    public class MemorySpaceTests
    {
        [Fact]
        public void ConstructorTests()
        {
            var m = new MemorySpace(0, 100);
            Assert.Equal(100u, m.Size);
            Assert.Equal(0u, m.StartAddress);
            Assert.Equal(99u, m.EndAddress);
            Assert.False(m.Free);
            Assert.Null(m.Next);
            Assert.Null(m.Previous);
            

            m = new MemorySpace(true, 0, 99);
            Assert.Equal(100u, m.Size);
            Assert.Equal(0u, m.StartAddress);
            Assert.Equal(99u, m.EndAddress);
            Assert.True(m.Free);
            Assert.Null(m.Next);
            Assert.Null(m.Previous);

            m = new MemorySpace(0, 100, false);
            Assert.Equal(100u, m.Size);
            Assert.Equal(0u, m.StartAddress);
            Assert.Equal(99u, m.EndAddress);
            Assert.False(m.Free);
            Assert.Null(m.Next);
            Assert.Null(m.Previous);

            
            var prev = new MemorySpace(0, 0);
            var next = new MemorySpace(0, 0);
            m = new MemorySpace(false, 0, 99, prev, next);
            Assert.Equal(100u, m.Size);
            Assert.Equal(0u, m.StartAddress);
            Assert.Equal(99u, m.EndAddress);
            Assert.False(m.Free);
            Assert.Equal(next, m.Next);
            Assert.Equal(prev, m.Previous);
            Assert.Equal(m, next.Previous);
            Assert.Equal(m, prev.Next);

            prev = new MemorySpace(0, 0);
            next = new MemorySpace(0, 0);
            m = new MemorySpace(0, 100,false, prev, next);
            Assert.Equal(100u, m.Size);
            Assert.Equal(0u, m.StartAddress);
            Assert.Equal(99u, m.EndAddress);
            Assert.False(m.Free);
            Assert.Equal(next, m.Next);
            Assert.Equal(prev, m.Previous);
            Assert.Equal(m, next.Previous);
            Assert.Equal(m, prev.Next);

        }

        [Fact]
        public void SizeChange()
        {
            var m = new MemorySpace(0, 100);
            Assert.Equal(100u, m.Size);
            Assert.Equal(0u, m.StartAddress);
            Assert.Equal(99u, m.EndAddress);
            Assert.False(m.Free);
            Assert.Null(m.Next);
            Assert.Null(m.Previous);

            m.Size = 30;

            Assert.Equal(30u, m.Size);
            Assert.Equal(0u, m.StartAddress);
            Assert.Equal(29u, m.EndAddress);
            Assert.False(m.Free);
            Assert.Null(m.Next);
            Assert.Null(m.Previous);
        }
    }
}
