using System;
using System.Collections;
using SL.Common;
using SL.Interactions;
using SL.Signals;
using UnityEngine;

namespace SL.Game.Bonfires
{
    public class Bonfire : MessengerBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private InteractionTrigger _trigger;

        private IDisposable _subscription;
        private bool _playerInto;

        public string Id => _id;
        public Transform SpawnPoint => _spawnPoint;

        private void Awake()
        {
            Subscribe<InteractionMessage>(OnInteractionMessage);
        }

        private void OnInteractionMessage(InteractionMessage message)
        {
            if(_playerInto == false)
                return;

            Publish(new BonfireInteractedMessage(_id, transform.position));
            StartCoroutine(DelayedStandUp());
        }

        private IEnumerator DelayedStandUp()
        {
            yield return new WaitForSeconds(10f);
            Publish(new BonfireLeaveMessage(_id));
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

        private void OnEntered(Interactor interactor)
            => _playerInto = interactor.GetComponent<Player.Player>() != null;

        private void OnExited(Interactor interactor)
            => _playerInto = false;
    }
}