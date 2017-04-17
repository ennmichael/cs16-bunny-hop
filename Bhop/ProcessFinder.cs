using System.Diagnostics;

namespace BHop
{
    class ProcessFinder
    {
        public static Process FindProcessByName(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes.Length != 1)
                return null;
            return processes[0];
        }
    }
}
