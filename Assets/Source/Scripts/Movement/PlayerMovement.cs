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
        private MovementState _state = MovementState.Stay;

        public event Action Moving;
        public event Action Staying;
        public event Action Dodging;

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
                case MovementState.Stay:
                    break;
                case MovementState.Move:
                    SelfMoving();
                    break;
                case MovementState.Dodge:
                    SelfDodging();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnStaying()
        {
            if (_state == MovementState.Dodge)
                return;

            _state = MovementState.Stay;
            Staying?.Invoke();
        }

        private void OnMoving()
        {
            if (_state is MovementState.Move or MovementState.Dodge)
                return;

            _state = MovementState.Move;
            Moving?.Invoke();
        }

        private void OnDodging()
        {
            if (_state == MovementState.Dodge)
                return;

            if (_input.Direction.sqrMagnitude > 0f)
                StartDodging(_rollingDistance, _input.Direction);
            else
                StartDodging(_stepBackDistance, -_model.forward);

            _dodgeDistanceProgress = 0f;
            _state = MovementState.Dodge;
        }
    
        private void StartDodging(float distance, Vector3 direction)
        {
            _dodgeDistance = distance;
            _dodgeDirection = direction;
            
            Debug.Log("Movement Dodge");
            Dodging?.Invoke();
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

        private void SelfDodging()
        {
            if (_dodgeDistanceProgress < _dodgeDistance)
            {
                Vector3 offset = _dodgeDirection * (_speed * Time.deltaTime);
                transform.Translate(offset);
                _dodgeDistanceProgress += offset.magnitude;
            }
            else
            {
                _state = MovementState.Stay;
            }
        }
    }
}