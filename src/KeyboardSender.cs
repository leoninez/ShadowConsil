using System;

namespace ShadowConsil.src
{
    public class KeyboardSender
    {
        public ProcessInfo Info { get; set; }

        public KeyboardSender(ProcessInfo processInfo)
        {
            this.Info = processInfo;
        }

        // example code for backup
        private void ExampleKeyDownForNotepad(System.Windows.Forms.Keys key)
        {
            IntPtr hWndNotepad = NativeMethods.FindWindow("Notepad", null);
            IntPtr hWndEdit = NativeMethods.FindWindowEx(hWndNotepad, IntPtr.Zero, "Edit", null);
            NativeMethods.SendMessage(hWndEdit, NativeMethods.WM_SETTEXT, 0, " key");
            NativeMethods.PostMessage(hWndNotepad, NativeMethods.WM_KEYDOWN, key, IntPtr.Zero);
        }

        public void KeyDown(System.Windows.Forms.Keys key)
        {
            IntPtr hWndMain  = this.Info.raw.MainWindowHandle;
            NativeMethods.PostMessage(hWndMain, NativeMethods.WM_KEYDOWN, key, IntPtr.Zero);
        }

        public void KeyUp(System.Windows.Forms.Keys key)
        {
            IntPtr hWndMain = this.Info.raw.MainWindowHandle;
            NativeMethods.PostMessage(hWndMain, NativeMethods.WM_KEYUP, key, IntPtr.Zero);
        }
    }
}
