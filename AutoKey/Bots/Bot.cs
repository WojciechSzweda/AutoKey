using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKey.Bots
{
    interface IBot
    {
        bool isActive { get; }
        int ActivatorvkCode { get; }

        void Activate();
        void Deactivate();
        void ToggleActivity();
        void SetupActivatorListening();
    }

    abstract public class Bot : IBot
    {
        public bool isActive { get; protected set; }
        public int ActivatorvkCode { get; private set; }

        abstract public void Activate();
        abstract public void Deactivate();

        public Bot()
        {
            this.isActive = false;
        }

        public Bot(int activatorvkCode) : this()
        {
            this.ActivatorvkCode = activatorvkCode;
        }

        public virtual void ToggleActivity()
        {
            if (this.isActive)
                Deactivate();
            else
                Activate();
            BotHelper.BotStatusIndication(this.GetType().Name, isActive);

        }

        public void GetNextPressedKeyAsActivator(object sender, KeyPressedEventArgs e)
        {
            ActivatorvkCode = e.vkCode;
            KeyboardHook.KeyPressedEvent -= GetNextPressedKeyAsActivator;
            BotHelper.VkCodeSet(e.vkCode);
        }

        public void SetupActivatorListening()
        {
            KeyboardHook.ClearKeyPressedEvent();
            KeyboardHook.KeyPressedEvent += GetNextPressedKeyAsActivator;
            BotHelper.WaitingForKeyPressIndicator();
        }
    }



}
