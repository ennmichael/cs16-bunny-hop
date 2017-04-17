using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BHop
{
    class EditableProcess: SafeHandle
    {
        public static EditableProcess ByName(string processName)
        {
            var process = ProcessFinder.FindProcessByName(processName);
            if (process == null)
                return null;
            return new EditableProcess(process);
        }

        private EditableProcess(Process process)
                : base(new IntPtr(0), true)
        {
            processId = process.Id;
            processHandle = OpenProcessHandleFromId(process.Id);
        }

        private static IntPtr OpenProcessHandleFromId(int id)
        {
            // Move these ugly shit constaints into a single Read-write named constant
            return Kernel32.OpenProcess(Kernel32.PROCESS_WM_READ | Kernel32.PROCESS_VM_WRITE | Kernel32.PROCESS_VM_OPERATION, false, id);
        }

        public override bool IsInvalid => processHandle == new IntPtr(0);

        protected override bool ReleaseHandle()
        {
            return Kernel32.CloseHandle(processHandle);
        }

        public void WriteBytesToAddress(IntPtr addressInHex, byte[] bytes)
        {
            int bytesWritten = 0; // TODO Use this to check that everything went well rather than just ignoring errors
            Kernel32.WriteProcessMemory(processHandle, addressInHex, bytes, bytes.Length, ref bytesWritten);
        }

        public byte[] ReadBytesFromAddress(IntPtr addressInHex, int numberOfBytesToRead)
        {
            var result = new byte[numberOfBytesToRead];
            int numberOfBytesRead = 0;
            Kernel32.ReadProcessMemory(processHandle, addressInHex, result, result.Length, ref numberOfBytesRead);
            return result;
        }

        public int GetModuleBaseAddress(string moduleName)
        {
            return ModuleLookup.GetModuleBaseAddress(processId, moduleName);
        }

        private readonly IntPtr processHandle;
        private readonly int processId;
    }
}
