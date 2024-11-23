using System;
using SL.Health.Models;
using SL.Movement;
using UnityEngine;
using Zenject;

namespace SL.General.Player
{
    public class Player : MonoBehaviour, IHealthOwner
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerAnimator _animator;

        private HealthModel _healthModel;
#if UNITY_EDITOR

        [ContextMenu(nameof(SitDown))]
        private void SitDown() => _movement.SitDown();

        [ContextMenu(nameof(StandUp))]
        private void StandUp() => _movement.StandUp();
#endif

        [Inject]
        private void Construct(HealthModel model)
        {
            _healthModel = model;
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
    }
}