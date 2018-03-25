using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoKey
{
    public class BreakToken
    {
        public bool shouldBreak = false;
    }


    public static partial class SendInputApi
    {
        static Random rng = new Random();

        /// <summary>
        /// Simulate typing text, keyboard input is delayed
        /// </summary>
        /// <param name="txt">text to type</param>
        /// <param name="token">break token</param>
        public static void SendText(string txt, BreakToken token)
        {
            //INPUT[] Inputs = createInputArray(txt);
            //foreach (var input in Inputs)
            //{
            //    if (token.shouldBreak)
            //        return;

            //    Thread.Sleep(50);
            //    Send(input);
            //}

            foreach (var c in txt)
            {
                if (token.shouldBreak)
                    return;
                if (keyMapAction.ContainsKey(c))
                    keyMapAction[c]();
                Thread.Sleep(40);
            }
        }

        /// <summary>
        /// Simulate typing text asynchronously
        /// </summary>
        /// <param name="txt">text to type</param>
        /// <param name="token">break token</param>
        public async static void SendTextAsync(string txt, BreakToken token)
        {
            INPUT[] Inputs = createInputArray(txt);
            foreach (var input in Inputs)
            {
                if (token.shouldBreak)
                    return;

                await Task.Delay(30);
                Send(input);
            }
        }

        /// <summary>
        /// Simulate pressing special key i.e: UP ARROW
        /// </summary>
        /// <param name="key">special key</param>
        public static void SendSpecialKey(SpecialKeySCS key)
        {
            INPUT input = new INPUT();
            input.type = 1;
            input.U.ki.wScan = (ScanCodeShort)key;
            input.U.ki.dwFlags = KEYEVENTF.EXTENDEDKEY | KEYEVENTF.SCANCODE;

            KeyboardHook.SendTroughApi();
            SendInput((uint)1, new INPUT[] { input }, INPUT.Size);
        }

        /// <summary>
        /// Simulate pressing one key
        /// </summary>
        /// <param name="key">pressed key</param>
        public static void SendKey(char key)
        {
            SendKeyDown(key);
            Thread.Sleep(10);
            SendKeyUp(key);
        }

        /// <summary>
        /// Simulate pressing one key
        /// </summary>
        /// <param name="key">pressed key</param>
        private static void SendKey(ScanCodeShort scs)
        {
            SendKeyDown(scs);
            Thread.Sleep(10);
            SendKeyUp(scs);
        }


        public static void SendKeyDown(char key)
        {
            Send(new INPUT(1, keyMap[key], KEYEVENTF.SCANCODE));
        }

        public static void SendKeyUp(char key)
        {
            Send(new INPUT(1, keyMap[key], KEYEVENTF.SCANCODE | KEYEVENTF.KEYUP));
        }

        private static void SendKeyDown(ScanCodeShort scs)
        {
            Send(new INPUT(1, scs, KEYEVENTF.SCANCODE));
        }

        private static void SendKeyUp(ScanCodeShort scs)
        {
            Send(new INPUT(1, scs, KEYEVENTF.SCANCODE | KEYEVENTF.KEYUP));
        }

        public static void SendButton(ButtonSCS btn)
        {
            Send(new INPUT(1, (ScanCodeShort)btn, KEYEVENTF.SCANCODE));
            Thread.Sleep(20);
            Send(new INPUT(1, (ScanCodeShort)btn, KEYEVENTF.SCANCODE | KEYEVENTF.KEYUP));
        }


        /// <summary>
        /// Send keys combination
        /// </summary>
        /// <param name="extendKey">extented key</param>
        /// <param name="key">key to use with extended key</param>
        public static void SendExtCombination(ExtendedSCS extendKey, char key)
        {
            if (!keyMap.ContainsKey(key))
                return;

            var extKeyDown = new INPUT(1, (ScanCodeShort)extendKey, KEYEVENTF.EXTENDEDKEY | KEYEVENTF.SCANCODE);
            var keyWithExt = new INPUT(1, keyMap[key], KEYEVENTF.SCANCODE);
            var extKeyUp = new INPUT(1, (ScanCodeShort)extendKey, KEYEVENTF.KEYUP | KEYEVENTF.SCANCODE | KEYEVENTF.EXTENDEDKEY);

            Send(extKeyDown);
            Thread.Sleep(20);
            Send(keyWithExt);
            Thread.Sleep(20);
            Send(extKeyUp);

        }

        public static void SendButtonCombination(ButtonSCS button, char key)
        {
            if (!keyMap.ContainsKey(key))
                return;

            var btnDown = new INPUT(1, (ScanCodeShort)button, KEYEVENTF.SCANCODE);
            var keyWithBtn = new INPUT(1, keyMap[key], KEYEVENTF.SCANCODE);
            var btnUp = new INPUT(1, (ScanCodeShort)button, KEYEVENTF.KEYUP | KEYEVENTF.SCANCODE);

            Send(btnDown);
            Thread.Sleep(20);
            SendKey(key);
            Thread.Sleep(20);
            Send(btnUp);

        }

        public static void SendExtentedKey(ExtendedSCS extendKey)
        {

            var extKeyDown = new INPUT(1, (ScanCodeShort)extendKey, KEYEVENTF.EXTENDEDKEY | KEYEVENTF.SCANCODE);
            var extKeyUp = new INPUT(1, (ScanCodeShort)extendKey, KEYEVENTF.KEYUP | KEYEVENTF.SCANCODE | KEYEVENTF.EXTENDEDKEY);

            Send(extKeyDown);
            Thread.Sleep(20);
            Send(extKeyUp);

        }

        private static INPUT[] createInputArray(string txt)
        {
            List<INPUT> inputs = new List<INPUT>();
            for (int i = 0; i < txt.Length; i++)
            {
                if (!keyMap.ContainsKey(txt[i]))
                    continue;
                var input = new INPUT(1, keyMap[txt[i]], KEYEVENTF.SCANCODE);
                inputs.Add(input);
            }

            return inputs.ToArray();
        }

        private static void Send(INPUT input)
        {
            KeyboardHook.SendTroughApi();
            SendInput((uint)1, new INPUT[] { input }, INPUT.Size);
        }









    }

}
