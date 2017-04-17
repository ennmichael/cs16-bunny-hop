using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHop
{
    class SpacebarKey
    {
        public static SpacebarKey FromEditableProcess(EditableProcess editableProcess)
        {
            return new SpacebarKey(editableProcess);
        }

        private SpacebarKey(EditableProcess editableProcess)
        {
            var spacebarAddress = new AddressInModule(editableProcess, spacebarAddressModule, spacebarAddressOffset);
            this.spacebarAddressReader = new AddressReader(spacebarAddress);
        }

        public bool IsDown()
        {
            return spacebarAddressReader.ReadSingleByte() == 1;
        }

        private readonly string spacebarAddressModule = "hw.dll";
        private readonly IntPtr spacebarAddressOffset = new IntPtr(0xA7FBE0);

        private readonly AddressReader spacebarAddressReader;
    }
}
