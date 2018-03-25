using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKey.Bots
{
    public class SpamKey : Bot
    {
        public char KeyToUse { get; set; }
        int delay = 100;

        public SpamKey(char key) : base()
        {
            this.KeyToUse = key;
        }

        public SpamKey(int activatorvkCode, char key) : base(activatorvkCode)
        {
            this.KeyToUse = key;
        }

        public override void Activate()
        {
            this.isActive = true;
            Working();
        }

        public override void Deactivate()
        {
            this.isActive = false;
        }

        async void Working()
        {
            await Task.Delay(delay);
            SendInputApi.SendKey(KeyToUse);
            if (isActive)
                this.Working();
        }
    }
}
