using System;
using Loxodon.Framework.Messaging;
using SL.Signals;
using UnityEngine;
using Zenject;

namespace SL.Interactions
{
    public class InteractionTrigger : MonoBehaviour
    {
        [SerializeField] private string _text;
        [SerializeField] private Sprite _icon;

        private IMessenger _messenger;

        public event Action<Interactor> Entered;
        public event Action<Interactor> Exited;

        [Inject]
        private void Construct(IMessenger messenger)
        {
            _messenger = messenger;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Interactor interactor) == false)
                return;

            _messenger.Publish(new ShowInteractionMessage(_text, _icon));
            Entered?.Invoke(interactor);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Interactor interactor) == false)
                return;

            _messenger.Publish(new HideInteractionMessage());
            Exited?.Invoke(interactor);
        }
    }
}