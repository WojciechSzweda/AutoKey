using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoKey.Bots
{
    public class TextSubstitution : Bot
    {
        public string SubtiteString { get; private set; }
        public int Index { get; private set; }

        private delegate void Test();
        private Test IndexHandler;

        public TextSubstitution(int vkCode, string txt, bool infinite = true) : base(vkCode)
        {
            SubtiteString = txt;
            Index = 0;
            if (infinite)
                IndexHandler = IndexHandlerInfinite;
            else
                IndexHandler = IndexHandlerFinite;

        }

        public void SetNewString(string txt)
        {
            SubtiteString = txt;
            Index = 0;
        }

        private void IndexHandlerInfinite()
        {
            Index %= SubtiteString.Length;
        }

        private void IndexHandlerFinite()
        {
            if (Index >= SubtiteString.Length)
                this.Deactivate();
        }

        public void GetNextKey()
        {
            IndexHandler();
            if (isActive)
                SendInputApi.SendKey(SubtiteString[Index]);
            Index++;
        }

        public override void Activate()
        {
            isActive = true;
            Index = 0;
        }

        public override void Deactivate()
        {
            isActive = false;
        }
    }
}
