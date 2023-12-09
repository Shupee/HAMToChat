using BuildSoft.VRChat.Osc;
using HR.Functions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HAMToChat.Functions
{
    internal class AFKDetect
    {
        public static AFKDetect Instance;
        public AFKDetect()
        {
            KeyPressedHandler = KeyPressedCallback;
            _keyboardHookId = SetHookKeyboard(proc);
            _mouseHookId = SetHookMouse(proc);
            OscParameter.ValueChanged += (sender, e) =>
            {
                foreach (var item in sender)
                {
                    if (item.Key == "/avatar/parameters/AFK")
                    {
                        _VRCAFK = (bool)item.Value;
                    }
                }
            };
            new Waiter(delegate {
                _waitTimeInSecund += 1;
                _appAFK = _waitTimeInSecund >= 60;
            },1000).Start();
            Instance = this;
            //string time = DateTime.Now.ToString("h:mm:ss tt");
            //Console.WriteLine("The current time is {0}", time);
        }
        public bool IsVR()
        {

            var steamVR = Process.GetProcessesByName("vrserver");
            var Oculus = Process.GetProcessesByName("OculusClient");
            if (steamVR.Length > 0 || Oculus.Length > 0)
                return true;
            return false;
        }
        public bool IsAFK()
        {
            if (IsVR())
            {
                if (_VRCAFK)
                {
                    return true;
                }
                return false;
            }
            if (_appAFK || _VRCAFK)
            {
                return true;
            }
            return false;
        }
        public string GetAFKTime()
        {
            string str = "0";
            TimeSpan time = TimeSpan.FromSeconds(_appAFK ? _waitTimeInSecund - 60 : _waitTimeInSecund);
            if (time.Hours > 0)
            {
                str = time.ToString(@"hh\:mm\:ss");
            }
            else if (time.Seconds > 0)
            {
                str = time.ToString(@"mm\:ss");
            }
            return str;
        }
        private static int _waitTimeInSecund = 0;
        private bool _VRCAFK;
        private bool _appAFK;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WH_MOUSE_LL = 14;

        private static IntPtr _keyboardHookId = IntPtr.Zero;
        private static IntPtr _mouseHookId = IntPtr.Zero;
        private static readonly LowLevelProc proc = HookCallback;

        private delegate void KeyPressedDelegate(int key);
        private static KeyPressedDelegate KeyPressedHandler;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);

        public void unhook()
        {
            UnhookWindowsHookEx(_mouseHookId);
            UnhookWindowsHookEx(_keyboardHookId);
        }

        private static IntPtr SetHookMouse(LowLevelProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }      
        private static IntPtr SetHookKeyboard(LowLevelProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                KeyPressedHandler?.Invoke(vkCode);
                return CallNextHookEx(_keyboardHookId, nCode, wParam, lParam);
            }
            else
            {
                int vkCode = Marshal.ReadInt32(lParam);
                KeyPressedHandler?.Invoke(vkCode);
                return CallNextHookEx(_mouseHookId, nCode, wParam, lParam);
            }
        }

        private static void KeyPressedCallback(int key)
        {
            if (!Instance._VRCAFK)
            {
                _waitTimeInSecund = 0;
            }
        }
    }
}
