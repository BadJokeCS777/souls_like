using Loxodon.Framework.Messaging;
using SL.Common;
using SL.Signals;
using UnityEngine;

namespace SL.Game.Bonfires
{
    internal class BonfireManager : MessengerBase
    {
        private const string LastBonfireIdKey = nameof(LastBonfireId);

        public string LastBonfireId { get; private set; }

        public BonfireManager(IMessenger messenger) : base(messenger) { }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Subscribe<BonfireInteractedMessage>(OnBonfireInteractedMessage);

            LastBonfireId = PlayerPrefs.GetString(LastBonfireIdKey, string.Empty);
        }

        private void OnBonfireInteractedMessage(BonfireInteractedMessage message)
        {
            LastBonfireId = message.Id;
            PlayerPrefs.SetString(LastBonfireIdKey, LastBonfireId);
        }
    }
}