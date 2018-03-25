using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKey.Bots
{
    public class AutoRun : Bot
    {
        public char KeyToUse { get; set; }

        public AutoRun(char key) : base()
        {
            this.KeyToUse = key;
        }

        public AutoRun(int activatorvkCode, char key) : base(activatorvkCode)
        {
            this.KeyToUse = key;
        }

        public override void Activate()
        {
            this.isActive = true;
            SendInputApi.SendKeyDown(this.KeyToUse);
        }

        public override void Deactivate()
        {
            this.isActive = false;
            SendInputApi.SendKeyUp(this.KeyToUse);
        }


    }
}
