using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kernel
{
    public class MemorySpace
    {
        public MemorySpace(uint startAddress, uint size)
        {
            this.StartAddress = startAddress;
            this.Size = size;
            this.EndAddress = startAddress + size;
        }

        public uint StartAddress { get; private set; }
        public uint EndAddress { get; private set; }
        public uint Size { get; private set; }

        public bool Contains(uint address)
        {
            return (StartAddress >= address && address <= EndAddress);
        }

        public bool Intersects(MemorySpace space)
        {
            return ((this.Contains(space.StartAddress) || this.Contains(space.EndAddress)) ||
                    (space.Contains(this.StartAddress) || space.Contains(this.EndAddress)));
        }
    }
}
