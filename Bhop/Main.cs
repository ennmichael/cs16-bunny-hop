using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace BHop
{
    class MainClass
    {
        public static bool playerIsOnGround(EditableProcess editableProcess)
        {
            var address = new AddressInModule(editableProcess, "hw.dll", new IntPtr(0x11FDAB4)); // That should not be an intptr
            var addressReader = new AddressReader(address);
            return addressReader.ReadSingleByte() == 1;
        }

        public static void makePlayerJump(EditableProcess editableProcess)
        {
            var address = new AddressInModule(editableProcess, "client.dll", new IntPtr(0x12D048)); // That should not be an intptr
            var addressReader = new AddressReader(address);
            addressReader.WriteSingleByte((byte)5); // Lol. reader.write.
            Thread.Sleep(15); // Arbitrary
            addressReader.WriteSingleByte((byte)4);
        }

        static void Main(string[] args)
        {
            var editableProcess = EditableProcess.ByName("hl");
            var spacebar = SpacebarKey.FromEditableProcess(editableProcess);

            for (;;)
                while (spacebar.IsDown())
                {
                    if (playerIsOnGround(editableProcess))
                    {
                        makePlayerJump(editableProcess);
                    }
                }
        }   
    }
}
