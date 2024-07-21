using System;
using UnityEngine;

namespace SL.Movement.States
{
    internal class DodgeState : State.State
    {
        private readonly float _speed;
        private readonly Transform _transform;
        private readonly Dodge _dodge;
        private readonly Action _callback;

        public DodgeState(float speed, Transform transform, Dodge dodge, Action callback)
        {
            _speed = speed;
            _transform = transform;
            _dodge = dodge;
            _callback = callback;
        }

        public override void Begin() { }

        public override void Update()
        {
            if (_dodge.IsProcessing)
            {
                Vector3 offset = _dodge.Direction * (_speed * Time.deltaTime);
                _transform.Translate(offset);
                _dodge.Update(offset.magnitude);
            }
            else
            {
                _callback?.Invoke();
            }
        }
    }
}