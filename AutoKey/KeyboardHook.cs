using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using AutoKey.Bots;
using System.Collections.Generic;

namespace AutoKey
{

    class KeyboardHook : IDisposable
    {
        public static event EventHandler<KeyPressedEventArgs> KeyPressedEvent;

        private static Random rng = new Random();
        private static bool sendThroughApi = false;


        public KeyboardHook()
        {
            _hookID = SetHook(_proc);
            TextHax.Activate();
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        private static TextSubstitution TextHax = new TextSubstitution(107, "trolololololo");
        private static List<IBot> BotList = new List<IBot> { new AutoRun('w'), new SpamKey('w'), TextHax };

        public static void AddBot(IBot bot)
        {
            BotList.Add(bot);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Console.WriteLine($"vk: {vkCode}  {KeyStatus(wParam)}");

            if (KeyPressedEvent != null)
            {
                KeyPressedEvent.Invoke(null, new KeyPressedEventArgs { vkCode = vkCode });
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }

            if (nCode >= 0 && !sendThroughApi && wParam == (IntPtr)WM_KEYDOWN)
            {

                BotList.FirstOrDefault(bot => vkCode == bot.ActivatorvkCode)?.ToggleActivity();

            }
            if (sendThroughApi)
            {
                sendThroughApi = false;
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }
            if (TextHax.isActive && wParam == (IntPtr)WM_KEYDOWN)
            {
                TextHax.GetNextKey();
                return (IntPtr)1;
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);



        }

        public static void SendTroughApi()
        {
            sendThroughApi = true;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public static void ClearKeyPressedEvent()
        {
            KeyPressedEvent = null;
        }

        private static string KeyStatus(IntPtr wParam)
        {
            return wParam == (IntPtr)WM_KEYDOWN ? "down" : wParam == (IntPtr)WM_KEYUP ? "up" : "other??";
        }
    }

    public class KeyPressedEventArgs : EventArgs
    {
        public int vkCode;
    }
}