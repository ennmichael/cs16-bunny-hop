using System;

namespace BHop
{
    class JumpCommand
    {
        public static JumpCommand fromEditableProcess(EditableProcess editableProcess)
        {
            return new JumpCommand(editableProcess);
        }

        private JumpCommand(EditableProcess editableProcess)
        {
            this.editableProcess = editableProcess;
        }

        private readonly IntPtr jumpCommandAddressInHex = new IntPtr(0); // TODO
        private readonly EditableProcess editableProcess;
    }
}
