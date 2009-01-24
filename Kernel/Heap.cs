using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kernel
{
    /// <summary>
    /// System used to allocate memory
    /// </summary>
    public class Heap : MemorySegment
    {
        private Heap(uint startAddress, uint size)
            : base(startAddress, size)
        {
        }

        public static Heap Instance = new Heap(0, 100);
        
    }
}
