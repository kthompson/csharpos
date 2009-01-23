using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kernel
{
    /// <summary>
    /// System used to allocate memory
    /// </summary>
    public class Heap 
    {
        private MemorySpace _first;

        public MemorySpace All { get; private set; }

        private Heap(uint startAddress, uint size)
        {
            this.All = new MemorySpace(startAddress, size);
        }

        public MemorySpace AllocateMemory(uint size)
        {
            if (_first == null)
            {
                if (size > All.Size)
                    throw new OutOfMemoryException();

                _first = new MemorySpace(All.StartAddress, size);
                return _first;
            }

            MemorySpace n = _first;
            while (n.Next != null)
                n = n.Next;

            if (n.EndAddress + size > All.EndAddress)
                throw new OutOfMemoryException();

            n.Next = new MemorySpace(n.EndAddress + 1, size);

            return n.Next;
        }

        public void FreeMemory(MemorySpace space)
        {
            MemorySpace prev = space.Previous;
            MemorySpace next = space.Next;
            if (prev != null)
                prev.Next = next;
            
            if (next != null)
                next.Previous = prev;
        }

        public static Heap Instance = new Heap(0,0);
        
    }
}
