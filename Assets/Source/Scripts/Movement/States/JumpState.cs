using System;
using UnityEngine;

namespace SL.Movement.States
{
    internal class JumpState : State.State
    {
        private const float GroundCheckDistance = 0.1f;
        
        private readonly float _height;
        private readonly Rigidbody _rigidbody;
        private readonly Transform _checkPoint;
        private readonly Action _onGroundCallback;

        private bool _maxHeightReached;
        private float _targetHeight;

        public JumpState(float height, Rigidbody rigidbody, Transform checkPoint, Action onGroundCallback)
        {
            _height = height;
            _rigidbody = rigidbody;
            _checkPoint = checkPoint;
            _onGroundCallback = onGroundCallback;
        }

        public bool OnGround => Physics.Raycast(_checkPoint.position, Vector3.down, GroundCheckDistance);

        public override void Begin()
        {
            if (OnGround == false)
                throw new AggregateException("Character must be on ground");
            
            Jump();
        }

        public override void Update()
        {
            if (OnGround)
                _onGroundCallback.Invoke();
        }

        public void Jump()
        {
            var mass = _rigidbody.mass;
            var velocity = (float)Math.Sqrt(2 * Math.Abs(Physics.gravity.y) * _height);
            var impulse = mass * velocity;

            _rigidbody.AddForce(Vector3.up * impulse, ForceMode.Impulse);
        }
    }
}