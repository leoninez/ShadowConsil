﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ShadowConsil.src.NativeMethods;

namespace ShadowConsil.src
{
    public class KeyboardHook : IDisposable
    {
        bool Global = false;

        public delegate void LocalKeyEventHandler(Keys key, bool Shift, bool Ctrl, bool Alt);
        public event LocalKeyEventHandler KeyDown;
        public event LocalKeyEventHandler KeyUp;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct KBDLLHookStruct
        {
            public Int32 vkCode;
            public Int32 scanCode;
            public Int32 flags;
            public Int32 time;
            public Int32 dwExtraInfo;
        }

        private int HookID = 0;
        CallbackDelegate TheHookCB = null;

        //Start hook
        public KeyboardHook(bool Global)
        {
            this.Global = Global;
            TheHookCB = new CallbackDelegate(KeybHookProc);
            if (Global)
            {
                HookID = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, TheHookCB,
                    0, //0 for local hook. eller hwnd til user32 for global
                    0); //0 for global hook. eller thread for hooken

                Console.WriteLine("HOOK GLOBAL OK");
            }
            else
            {
                HookID = SetWindowsHookEx(HookType.WH_KEYBOARD, TheHookCB,
                   0, //0 for local hook. or hwnd to user32 for global
                 GetCurrentThreadId()); //0 for global hook. or thread for the hook
                Console.WriteLine("HOOK OK");
            }
        }

        bool IsFinalized = false;
        ~KeyboardHook()
        {
            if (!IsFinalized)
            {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }
        public void Dispose()
        {
            if (!IsFinalized)
            {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }

        //The listener that will trigger events
        private int KeybHookProc(int Code, int W, int L)
        {
            KBDLLHookStruct LS = new KBDLLHookStruct();
            if (Code < 0)
            {
                return CallNextHookEx(HookID, Code, W, L);
            }
            try
            {
                if (!Global)
                {
                    if (Code == 0)
                    {
                        IntPtr ptr = IntPtr.Zero;

                        int keydownup = L >> 30;
                        if (keydownup == 0)
                        {
                            if (KeyDown != null) KeyDown((Keys)W, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                        }
                        if (keydownup == -1)
                        {
                            if (KeyUp != null) KeyUp((Keys)W, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                        }
                        //System.Diagnostics.Debug.WriteLine("Down: " + (Keys)W);
                    }
                }
                else
                {
                    KeyEvents kEvent = (KeyEvents)W;

                    Int32 vkCode = Marshal.ReadInt32((IntPtr)L); //Leser vkCode som er de første 32 bits hvor L peker.

                    if (kEvent != KeyEvents.KeyDown && kEvent != KeyEvents.KeyUp && kEvent != KeyEvents.SKeyDown && kEvent != KeyEvents.SKeyUp)
                    {
                    }
                    if (kEvent == KeyEvents.KeyDown || kEvent == KeyEvents.SKeyDown)
                    {
                        if (KeyDown != null) KeyDown((Keys)vkCode, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                    }
                    if (kEvent == KeyEvents.KeyUp || kEvent == KeyEvents.SKeyUp)
                    {
                        if (KeyUp != null) KeyUp((Keys)vkCode, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                //Ignore all errors...
            }

            return CallNextHookEx(HookID, Code, W, L);
        }

        public static bool GetCapslock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.CapsLock)) & true;
        }
        public static bool GetNumlock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.NumLock)) & true;
        }
        public static bool GetScrollLock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.Scroll)) & true;
        }
        public static bool GetShiftPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ShiftKey);
            if (state > 1 || state < -1) return true;
            return false;
        }
        public static bool GetCtrlPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ControlKey);
            if (state > 1 || state < -1) return true;
            return false;
        }
        public static bool GetAltPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.Menu);
            if (state > 1 || state < -1) return true;
            return false;
        }
    }
}
