using System;
using System.Collections;
using Loxodon.Framework.Messaging;
using SL.Interactions;
using SL.Signals;
using UnityEngine;
using Zenject;

namespace SL.General
{
    public class Bonfire : MonoBehaviour
    {
        [SerializeField] private InteractionTrigger _trigger;

        private Player.Player _player;
        private IMessenger _messenger;
        private IDisposable _subscription;

        [Inject]
        private void Construct(IMessenger messenger)
        {
            _messenger = messenger;
            _subscription = _messenger.Subscribe<InteractionMessage>(OnInteractionMessage);
        }

        private void OnInteractionMessage(InteractionMessage message)
        {
            if (_player == null)
                return;

            _player.SitDown();
            StartCoroutine(DelayedStandUp());
        }

        private IEnumerator DelayedStandUp()
        {
            yield return new WaitForSeconds(10f);
            _player.StandUp();
        }

        private void OnEnable()
        {
            _trigger.Entered += OnEntered;
            _trigger.Exited += OnExited;
        }

        private void OnDisable()
        {
            _trigger.Entered -= OnEntered;
            _trigger.Exited -= OnExited;
        }

        private void OnDestroy() => _subscription.Dispose();

        private void OnEntered(Interactor interactor)
        {
            _player = interactor.GetComponent<Player.Player>();
        }

        private void OnExited(Interactor interactor)
        {
            _player = null;
        }
    }
}