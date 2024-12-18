using UnityEngine;

namespace SL.Interactions
{
    public struct ShowInteractionMessage
    {
        public readonly InteractionTrigger Trigger;
        public readonly string Text;
        public readonly string ButtonText;
        public readonly Sprite ButtonIcon;

        public ShowInteractionMessage(InteractionTrigger trigger, string text, Sprite button)
        {
            Trigger = trigger;
            Text = text;
            ButtonIcon = button;
            ButtonText = string.Empty;
        }

        public ShowInteractionMessage(InteractionTrigger trigger, string text, string button)
        {
            Trigger = trigger;
            Text = text;
            ButtonText = button;
            ButtonIcon = null;
        }

    }
}