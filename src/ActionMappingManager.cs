using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ShadowConsil.src
{


    class ActionMappingManager
    {
        //https://blog.csdn.net/Andrewniu/article/details/80353244
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        // 安装钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        // 卸载钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        // 继续下一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        // 取得当前线程编号

        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();


        static int hKeyboardHook = 0;
        // handles
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                return 1;
            }
            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }
    }
}
