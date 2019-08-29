using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShadowConsil.src
{
    internal static class NativeMethods
    {
        public delegate int CallbackDelegate(int Code, int W, int L);

        public static readonly int WM_SETTEXT = 0x000C;
        public static readonly int WM_KEYDOWN = 0x0100;
        public static readonly int WM_KEYUP = 0x0101;

        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        public enum KeyEvents
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SKeyDown = 0x0104,
            SKeyUp = 0x0105
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, Keys wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(HookType idHook, CallbackDelegate lpfn, int hInstance, int threadId);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, int lParam);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        static public extern short GetKeyState(System.Windows.Forms.Keys nVirtKey);
    }
}
