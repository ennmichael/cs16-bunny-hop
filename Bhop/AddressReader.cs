using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHop
{
    class AddressReader
    {
        public AddressReader(Address address)
        {
            this.address = address;
        }

        public byte ReadSingleByte()
        {
            return address.ReadBytes(1)[0];
        }

        public int ReadInt()
        {
            var bytes = address.ReadBytes(sizeof(int));
            return BitConverter.ToInt32(bytes, 0);
        }

        public void WriteSingleByte(byte toWrite)
        {
            address.WriteBytes(new byte[1] { toWrite });
        }

        public void WriteInt(int toWrite)
        {
            address.WriteBytes(BitConverter.GetBytes(toWrite));
        }

        private readonly Address address;
    }
}
