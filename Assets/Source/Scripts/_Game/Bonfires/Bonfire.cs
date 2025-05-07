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

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public Transform SpawnPoint => _spawnPoint;

        private void Awake()
        {
            Subscribe<InteractionMessage>(OnInteractionMessage);
        }

        private void OnInteractionMessage(InteractionMessage message)
        {
            if (_trigger.IsInteractorLooking == false)
                return;

            Publish(new BonfireInteractedMessage(_id, transform.position));
            StartCoroutine(DelayedStandUp());
        }

        private IEnumerator DelayedStandUp()
        {
            yield return new WaitForSeconds(10f);
            Publish(new BonfireLeaveMessage(_id));
        }
    }
}