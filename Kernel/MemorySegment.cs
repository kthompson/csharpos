using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kernel
{
    public class MemorySegment
    {
        public MemorySpace First { get; protected set; }
        public uint Length { get; protected set; }

        protected MemorySegment(uint startAddress, uint size)
        {
            First = new MemorySpace(startAddress, size, true);
            this.Length = size;
        }

        private MemorySpace AllocateMemoryInternal(uint size)
        {
            MemorySpace n = First;
            while (n != null)
            {
                var nSize = n.Size;
                if (!n.Free || nSize < size) // not free or too small
                {
                    n = n.Next;
                }
                else if (nSize >= size) // free and big enough
                {
                    return Slice(size, n);
                }
            }
            return null;
        }

        private static MemorySpace Slice(uint size, MemorySpace n)
        {
            //size should always be >= n.Size

            uint nSize = n.Size;
            n.Free = false;
            if (nSize == size)
                return n;

            //resize it
            n.Size = size;
            //insert new memory space in between
            new MemorySpace(n.EndAddress + 1, nSize - n.Size, true, n, n.Next);
            return n;
        }

        public MemorySpace AllocateMemory(uint size)
        {
            MemorySpace n = AllocateMemoryInternal(size);
            if (n != null) return n;

            n = this.JoinFreeSpace(size);
            if (n != null) return n;

            throw new OutOfMemoryException();

        }

        private MemorySpace JoinFreeSpace(uint size)
        {
            MemorySpace n = First;
            while (n.Next != null)
            {
                if (n.Free && n.Next.Free)
                {
                    var nn = n.Next;
                    n.Size = n.Size + nn.Size;
                    n.Next = nn.Next;
                    n.Next.Previous = n;
                    //if this one is big enough now then return a slice
                    if (n.Size >= size)
                        return Slice(size, n);

                    //start over and search for more
                    n = First;
                }
                else
                {
                    n = n.Next;
                }
            }

            return null;
        }

        public void FreeMemory(MemorySpace space)
        {
            space.Free = true;
        }
    }
}
