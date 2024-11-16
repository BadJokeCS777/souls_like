using System;
using SL.Health.Models;
using SL.Movement;
using UnityEngine;
using Zenject;

namespace SL.General
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : MonoBehaviour, IHealthOwner
    {
        private HealthModel _healthModel;

        [Inject]
        private void Construct(HealthModel model)
        {
            _healthModel = model;
        }

        public void Init(Transform cameraTransform)
        {
            GetComponent<IMovement>().Init(cameraTransform);
        }

        public void ApplyDamage(float value)
        {
            if (value < 0f)
                throw new ArgumentException();

            _healthModel.Value -= value;
        }
    }
}