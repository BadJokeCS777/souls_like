using UnityEngine;

namespace SL.Signals
{
    public struct ShowInteractionMessage
    {
        public readonly string Text;
        public readonly Sprite Icon;

        public ShowInteractionMessage(string text, Sprite icon)
        {
            Text = text;
            Icon = icon;
        }
    }
}