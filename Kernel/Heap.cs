using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kernel
{
    /// <summary>
    /// System used to allocate memory
    /// </summary>
    public class Heap : MemorySpace
    {
        private List<MemorySpace> _allocatedMemory;

        private Heap(uint startAddress, uint size)
            : base(startAddress, size)
        {

#warning setup our memory start and end addresses from the kernel
            _allocatedMemory = new List<MemorySpace>();
        }

        public MemorySpace AllocateMemory(uint requiredSize)
        {
            // give first non-overlapping memoryspace
            int index = 0;

            MemorySpace test = new MemorySpace(this.StartAddress, requiredSize);

            while (!TestSpace(test)) 
            {
                if (index > _allocatedMemory.Count)
                    throw new OutOfMemoryException();

                test = new MemorySpace(_allocatedMemory[index].EndAddress + 1, requiredSize);
                index++;
            }

            _allocatedMemory.Add(test);
            return test;
        }

        private bool TestSpace(MemorySpace test)
        {
            for (var i = 0; i < _allocatedMemory.Count; i++)
                if (_allocatedMemory[i].Intersects(test))
                    return false;

            return true;
        }

        public void FreeMemory(MemorySpace space)
        {
            if (_allocatedMemory.Contains(space))
                _allocatedMemory.Remove(space);
            else
                throw new InvalidOperationException();
        }

        public static Heap Instance = new Heap(0,0);
        
    }
}
