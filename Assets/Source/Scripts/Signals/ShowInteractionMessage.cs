using UnityEngine;

namespace SL.Signals
{
    public struct ShowInteractionMessage
    {
        public readonly string Text;
        public readonly string ButtonText;
        public readonly Sprite ButtonIcon;

        public ShowInteractionMessage(string text, Sprite button)
        {
            Text = text;
            ButtonIcon = button;
            ButtonText = string.Empty;
        }

        public ShowInteractionMessage(string text, string button)
        {
            Text = text;
            ButtonText = button;
            ButtonIcon = null;
        }
    }
}