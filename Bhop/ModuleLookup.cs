using System;
using System.Runtime.InteropServices;

namespace BHop
{
    class ModuleLookup
    {
        [DllImport("moduleLookup.dll")]
        public static extern int GetModuleBaseAddress(int processId, [MarshalAs(UnmanagedType.LPTStr)]String moduleName);

        [DllImport("moduleLookup.dll")]
        public static extern int Test(int processId);

        [DllImport("moduleLookup.dll")]
        [return: MarshalAs(UnmanagedType.LPTStr)] public static extern String FormatErrorMessage(int errorCode);
    }
}
