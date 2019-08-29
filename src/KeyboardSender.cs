using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowConsil.src
{
    public class KeyboardSender
    {
        public ProcessInfo Info { get; set; }

        public KeyboardSender(ProcessInfo processInfo)
        {
            this.Info = processInfo;
        }

        public void KeyDown(System.Windows.Forms.Keys key, bool shift = false, bool ctrl = false, bool alt = false)
        {
            //Process p = selected.raw;
            //Console.WriteLine($"Write Message to {p.ToString()}");
            //SendMessage(p.MainWindowHandle, WM_KEYDOWN, (IntPtr)System.Windows.Forms.Keys.A, IntPtr.Zero);

            //IntPtr hWndNotepad = NativeMethods.FindWindow("Notepad", null);
            IntPtr hWndNotepad  = this.Info.raw.MainWindowHandle;
            
            //IntPtr hWndEdit = NativeMethods.FindWindowEx(hWndNotepad, IntPtr.Zero, "Edit", null);

            //NativeMethods.SendMessage(hWndEdit, WM_SETTEXT, 0, " key");

            NativeMethods.PostMessage(hWndNotepad, NativeMethods.WM_KEYDOWN, key, IntPtr.Zero);
        }

        public void KeyUp(System.Windows.Forms.Keys key, bool shift = false, bool ctrl = false, bool alt = false)
        {
            //Process p = selected.raw;
            //Console.WriteLine($"Write Message to {p.ToString()}");
            //SendMessage(p.MainWindowHandle, WM_KEYDOWN, (IntPtr)System.Windows.Forms.Keys.A, IntPtr.Zero);

            //IntPtr hWndNotepad = NativeMethods.FindWindow("Notepad", null);
            IntPtr hWndNotepad = this.Info.raw.MainWindowHandle;

            //IntPtr hWndEdit = NativeMethods.FindWindowEx(hWndNotepad, IntPtr.Zero, "Edit", null);

            //NativeMethods.SendMessage(hWndEdit, WM_SETTEXT, 0, " key");

            NativeMethods.PostMessage(hWndNotepad, NativeMethods.WM_KEYUP, key, IntPtr.Zero);
        }
    }
}
