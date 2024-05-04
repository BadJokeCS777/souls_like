using System;
using System.Collections;
using UnityEngine;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField, Range(0.01f, 1f)] private float _rotationSpeed = 0.5f;
        [SerializeField] private PlayerInput _input;
        [SerializeField] private Transform _model;
        [SerializeField] private float _rollingDistance = 3.5f;
        [SerializeField] private float _stepBackDistance = 1f;

        private MovementState _state;
        private Transform _camera;

        public event Action Moving;
        public event Action Staying;
        public event Action Rolling;
        public event Action StepBacking;

        private void Awake()
        {
            _state = MovementState.Staying;
            _camera = Camera.main.transform;
        }

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

            StartCoroutine(Movement());
            Moving?.Invoke();
        }

        private void OnDodging()
        {
            if (_state == MovementState.Dodging)
                return;

            if (_input.Direction.sqrMagnitude > 0f)
            {
                OnRolling();
                return;
            }
        
            OnStepBacking();
        }
    
        private void OnRolling()
        {
            StartCoroutine(Dodging(GetDirection(), _rollingDistance));
            Rolling?.Invoke();
        }

        private void OnStepBacking()
        {
            StartCoroutine(Dodging(-_model.forward, _stepBackDistance));
            StepBacking?.Invoke();
        }

        private IEnumerator Movement()
        {
            _state = MovementState.Moving;
        
            while (_state == MovementState.Moving)
            {
                Vector3 direction = GetDirection();
                transform.Translate(direction * (_speed * Time.deltaTime));

                if (direction.Equals(Vector3.zero) == false)
                {
                    _model.rotation = Quaternion.Slerp(_model.rotation, Quaternion.LookRotation(direction), _rotationSpeed);
                }
            
                yield return null;
            }   
        }

        private Vector3 GetDirection()
        {
            Vector3 direction = _camera.forward * _input.Direction.z + _camera.right * _input.Direction.x;
            direction.y = 0f;
            return direction.normalized;
        }

        private IEnumerator Dodging(Vector3 direction, float distance)
        {
            _state = MovementState.Dodging;
        
            float travelled = 0f;
        
            while (travelled < distance)
            {
                Vector3 offset = direction * (_speed * Time.deltaTime);
                transform.Translate(offset);
                travelled += offset.magnitude;
                yield return null;
            }
        
            _state = MovementState.Staying;
        }
    }
}