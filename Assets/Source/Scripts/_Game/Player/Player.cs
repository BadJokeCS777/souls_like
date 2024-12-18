using System;
using SL.Common;
using SL.Input;
using SL.Movement;
using SL.Signals;
using SL.UI.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SL.Game.Player
{
    public class Player : MessengerBehaviour, IHealthOwner
    {
        [SerializeField] private PlayerMovement _movement;

        private GameInput _gameInput;
        private HealthModel _healthModel;

        [Inject]
        private void Construct(HealthModel model)
        {
            _healthModel = model;

            _gameInput = new GameInput();
            _gameInput.Enable();
            _gameInput.Player.Interaction.performed += OnInteraction;

            Subscribe<BonfireInteractedMessage>(OnBonfireInteractedMessage);
            Subscribe<BonfireLeaveMessage>(OnBonfireLeaveMessage);
        }

        public void ApplyDamage(float value)
        {
            if (value < 0f)
                throw new ArgumentException();

            _healthModel.Value -= value;
        }

        private void OnInteraction(InputAction.CallbackContext ctx) => Publish(new InteractionMessage());

        private void OnBonfireInteractedMessage(BonfireInteractedMessage message)
            => _movement.BonfireSitDown(message.Position);

        private void OnBonfireLeaveMessage() => _movement.BonfireStandUp();
    }
}