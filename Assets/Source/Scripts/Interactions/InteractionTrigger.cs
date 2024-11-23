using Loxodon.Framework.Messaging;
using SL.Signals;
using UnityEngine;
using Zenject;

namespace SL.Interactions
{
    internal class InteractionTrigger : MonoBehaviour
    {
        [SerializeField] private string _text;
        [SerializeField] private Sprite _icon;

        private IMessenger _messenger;

        [Inject]
        private void Construct(IMessenger messenger)
        {
            _messenger = messenger;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Interactor>() == null)
                return;

            _messenger.Publish(new ShowInteractionMessage(_text, _icon));
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Interactor>() == null)
                return;

            _messenger.Publish(new HideInteractionMessage());
        }
    }
}