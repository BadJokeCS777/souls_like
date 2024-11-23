using System;
using Loxodon.Framework.Messaging;
using SL.Input;
using SL.Movement;
using SL.Signals;
using SL.UI.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SL.General.Player
{
    public class Player : MonoBehaviour, IHealthOwner
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerAnimator _animator;

        private GameInput _gameInput;
        private HealthModel _healthModel;
        private IMessenger _messenger;

        [Inject]
        private void Construct(HealthModel model, IMessenger messenger)
        {
            _healthModel = model;
            _messenger = messenger;

            _gameInput = new GameInput();
            _gameInput.Enable();
            //_gameInput.Player.Interaction.canceled += OnInteraction;
            _gameInput.Player.Interaction.performed += OnInteraction;
        }

        public void Init(Transform cameraTransform)
        {
            _movement.Init(cameraTransform, _animator.Animator);
        }

        public void ApplyDamage(float value)
        {
            if (value < 0f)
                throw new ArgumentException();

            _healthModel.Value -= value;
        }

        public void SitDown() => _movement.SitDown();

        public void StandUp() => _movement.StandUp();

        private void OnInteraction(InputAction.CallbackContext ctx) => _messenger.Publish(new InteractionMessage());
    }
}