using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKey.Bots
{
    class SpamButton : Bot
    {

        SendInputApi.ButtonSCS btn = SendInputApi.ButtonSCS.CONTROL;
        int delay = 100;

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
            SendInputApi.SendButton(btn);
            if (isActive)
                this.Working();
        }
    }
}
