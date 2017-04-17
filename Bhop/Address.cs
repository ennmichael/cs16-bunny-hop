using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHop
{
    class Address
    {
        public Address(EditableProcess editableProcess, IntPtr address)
        {
            this.editableProcess = editableProcess;
            this.address = address;
        }

        public void WriteBytes(byte[] bytes)
        {
            editableProcess.WriteBytesToAddress(address, bytes);
        }

        public byte[] ReadBytes(int numberOfBytesToRead)
        {
            return editableProcess.ReadBytesFromAddress(address, numberOfBytesToRead);
        }

        private readonly EditableProcess editableProcess;
        private readonly IntPtr address;
    }

    class AddressInModule : Address
    {
        public AddressInModule(EditableProcess editableProcess, string moduleName, IntPtr offset)
            : base(editableProcess, GetFullAddress(editableProcess, moduleName, offset))
        {
        }

        private static IntPtr GetFullAddress(EditableProcess editableProcess, string moduleName, IntPtr addressInHex)
        {
            return addressInHex + editableProcess.GetModuleBaseAddress(moduleName);
        }
    }
}
