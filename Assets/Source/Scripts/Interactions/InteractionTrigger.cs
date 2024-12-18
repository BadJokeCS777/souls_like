using System;
using SL.Common;
using SL.Signals;
using UnityEngine;

namespace SL.Interactions
{
    public class InteractionTrigger : MessengerBehaviour
    {
        [SerializeField] private string _text;
        [SerializeField] private Sprite _buttonIcon;
        [SerializeField] private string _buttonText;

        public event Action<Interactor> Entered;
        public event Action<Interactor> Exited;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Interactor interactor) == false)
                return;

            PublishShowMessage();

            Entered?.Invoke(interactor);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Interactor interactor) == false)
                return;

            Publish(new HideInteractionMessage());
            Exited?.Invoke(interactor);
        }

        private void PublishShowMessage()
        {
            if (_buttonIcon != null)
                Publish(new ShowInteractionMessage(_text, _buttonIcon));
            else if (string.IsNullOrEmpty(_buttonText) == false)
                Publish(new ShowInteractionMessage(_text, _buttonText));
        }
    }
}