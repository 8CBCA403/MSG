using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace splatoon3Tester.core
{
    public class MemoryInfo
    {
        private long addr; //64
        private long size; //64
        private MemoryType type; //32
        private int perm; //32

        public MemoryInfo(long addr, long size, int type, int perm)
        {
            this.addr = addr;
            this.size = size;
            this.type = (MemoryType)(type);
            this.perm = perm;
        }

        public long getAddress()
        {
            return addr;
        }

        public long getSize()
        {
            return size;
        }

        public long getNextAddress()
        {
            return addr + size;
        }

        public MemoryType getType()
        {
            return type;
        }

        public int getPerm()
        {
            return perm;
        }

        public bool isReadable()
        {
            return (perm & 1) != 0;
        }

        public bool isWriteable()
        {
            return (perm & 2) != 0;
        }

        public bool isExecutable()
        {
            return (perm & 4) != 0;
        }

        public bool contains(long addr)
        {
            return getAddress() <= addr && getNextAddress() > addr;
        }

        public override String ToString()
        {
            return "MemoryInfo{" +
                    "addr=0x" + Convert.ToString(addr, 16) +
                    ", size=0x" + Convert.ToString(size, 16) +
                    ", type=" + type +
                    ", perm=" + perm +
                    '}';
        }
    }
}
