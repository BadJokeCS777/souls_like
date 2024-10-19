using SL.States;
using UnityEngine;

namespace SL.Movement.States
{
    internal class MoveState : State
    {
        private readonly float _speed;
        private readonly float _rotationSpeedRatio;
        private readonly Transform _transform;
        private readonly Transform _model;
        private readonly IMovement _movement;

        public MoveState(float speed, float rotationSpeedRatio, Transform transform, Transform model, IMovement movement)
        {
            _speed = speed;
            _rotationSpeedRatio = rotationSpeedRatio;
            _transform = transform;
            _model = model;
            _movement = movement;
        }

        public override void Begin() { }

        public override void Update()
        {
            if (_movement.IsMoving == false)
                return;

            Vector3 direction = _movement.Direction;

            _transform.Translate(direction * (_speed * Time.deltaTime));
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _model.rotation = Quaternion.Slerp(_model.rotation, targetRotation, _rotationSpeedRatio);
        }
    }
}