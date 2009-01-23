using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kernel
{
    public class MemorySpace
    {
     

        public MemorySpace(bool free, uint startAddress, uint endAddress)
            : this(free, startAddress, endAddress, null, null)
        {
        }

        public MemorySpace(bool free, uint startAddress, uint endAddress, MemorySpace prev, MemorySpace next)
        {
            this.StartAddress = startAddress;
            this.EndAddress = endAddress;
            this.Free = free;

            this.SetAdjacentMemorySpaces(prev, next);
        }

        public MemorySpace(uint startAddress, uint size)
            : this(startAddress, size, false)
        {
        }

        public MemorySpace(uint startAddress, uint size, bool free)
            : this(startAddress, size, free, null, null)
        {
        }

        public MemorySpace(uint startAddress, uint size, bool free, MemorySpace prev, MemorySpace next)
        {
            this.StartAddress = startAddress;
            this.EndAddress = startAddress + size - 1;
            this.Free = free;

            this.SetAdjacentMemorySpaces(prev, next);
        }

        private void SetAdjacentMemorySpaces(MemorySpace prev, MemorySpace next)
        {
            if (prev != null)
            {
                this.Previous = prev;
                prev.Next = this;
            }

            if (next != null)
            {
                this.Next = next;
                next.Previous = this;
            }
        }

        public uint StartAddress { get; private set; }
        public uint EndAddress { get; private set; }

        public uint Size
        {
            get
            {
                return this.EndAddress - this.StartAddress + 1;
            }
            set
            {
                this.EndAddress = this.StartAddress + value - 1;
            }
        }

        public MemorySpace Previous { get; set; }
        public MemorySpace Next { get; set; }

        public bool Free { get; set; }
    }
}
