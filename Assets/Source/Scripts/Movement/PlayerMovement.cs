using System;
using UnityEngine;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField, Range(0.01f, 1f)] private float _rotationSpeedRatio = 0.5f;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private Transform _model;
        [SerializeField] private float _rollingDistance = 3.5f;
        [SerializeField] private float _stepBackDistance = 1f;

        private float _dodgeDistance;
        private float _dodgeDistanceProgress;
        private Vector3 _dodgeDirection;
        private MovementState _state = MovementState.Staying;

        public event Action Moving;
        public event Action Staying;
        public event Action Rolling;
        public event Action StepBacking;

        private void OnEnable()
        {
            _input.Moving += OnMoving;
            _input.Staying += OnStaying;
            _input.Dodging += OnDodging;
        }

        private void OnDisable()
        {
            _input.Moving -= OnMoving;
            _input.Staying -= OnStaying;
            _input.Dodging -= OnDodging;
        }

        private void Update()
        {
            switch (_state)
            {
                case MovementState.Staying:
                    break;
                case MovementState.Moving:
                    SelfMoving();
                    break;
                case MovementState.Dodging:
                    Dodging();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnStaying()
        {
            if (_state == MovementState.Dodging)
                return;

            _state = MovementState.Staying;
            Staying?.Invoke();
        }

        private void OnMoving()
        {
            if (_state is MovementState.Moving or MovementState.Dodging)
                return;

            _state = MovementState.Moving;
            Moving?.Invoke();
        }

        private void OnDodging()
        {
            if (_state == MovementState.Dodging)
                return;

            if (_input.Direction.sqrMagnitude > 0f)
                OnRolling();
            else
                OnStepBacking();

            _dodgeDistanceProgress = 0f;
            _state = MovementState.Dodging;
        }
    
        private void OnRolling()
        {
            SetDodgeParams(_rollingDistance, _input.Direction);
            Rolling?.Invoke();
        }

        private void OnStepBacking()
        {
            SetDodgeParams(_stepBackDistance, -_model.forward);
            StepBacking?.Invoke();
        }

        private void SetDodgeParams(float distance, Vector3 direction)
        {
            _dodgeDistance = distance;
            _dodgeDirection = direction;
        }

        private void SelfMoving()
        {
            Vector3 direction = _input.Direction;

            if (direction.sqrMagnitude <= 0f)
                return;
            
            transform.Translate(direction * (_speed * Time.deltaTime));
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _model.rotation = Quaternion.Slerp(_model.rotation, targetRotation, _rotationSpeedRatio);
        }

        private void Dodging()
        {
            if (_dodgeDistanceProgress < _dodgeDistance)
            {
                Vector3 offset = _dodgeDirection * (_speed * Time.deltaTime);
                transform.Translate(offset);
                _dodgeDistanceProgress += offset.magnitude;
            }
            else
            {
                _state = MovementState.Staying;
            }
        }
    }
}