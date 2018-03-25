using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKey.Bots
{
    public static class BotHelper
    {
        public static void BotStatusIndication(string name, bool state)
        {
            var stateString = state ? "Activated" : "Deactivated";
            var info = $"{name} {stateString}";
            WriteLine(info);
        }

        public static void VkCodeSet(int vkCode)
        {
            var info = $"Activator vkCode set: {vkCode}";
            WriteLine(info);
        }

        public static void WriteLine(string message)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }

        public static void WaitingForKeyPressIndicator()
        {
            WriteLine("Waiting for key...");
        }
    }
}
