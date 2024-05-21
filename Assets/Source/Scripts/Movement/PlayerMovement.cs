using System;
using Input;
using JetBrains.Annotations;
using Movement.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour, IMovement
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField, Range(0.01f, 1f)] private float _rotationSpeedRatio = 0.5f;
        [SerializeField] private Transform _model;
        [SerializeField] private float _rollingDistance = 3.5f;
        [SerializeField] private float _stepBackDistance = 1f;

        private GameInput _gameInput;
        private Transform _camera;
        private Vector3 _rawDirection;
        private Dodge _dodge;
        
        private State _currentState;
        private StayState _stayState;
        private MoveState _moveState;
        private DodgeState _dodgeState;

        public event Action Moving;
        public event Action Staying;
        public event Action Dodging;

        public bool IsMoving => _rawDirection.sqrMagnitude > 0f;
        
        public Vector3 Direction
        {
            get
            {
                Vector3 direction = _camera.forward * _rawDirection.z + _camera.right * _rawDirection.x;
                direction.y = 0f;
                return direction.normalized;
            }
        }
        
        private void Awake()
        {
            _camera = Camera.main.transform;
            _gameInput = new GameInput();
            _dodge = new Dodge();

            _stayState = new StayState();
            _moveState = new MoveState(_speed, _rotationSpeedRatio, transform, _model, this);
            _dodgeState = new DodgeState(_speed, transform, _dodge, OnDodgeCompleted);
            
            _currentState = _stayState;
        }

        private void OnEnable()
        {
            _gameInput.Enable();
            _gameInput.Player.Dodge.performed += OnDodging;
            _gameInput.Player.Move.performed += OnMoving;
            _gameInput.Player.Move.canceled += OnStaying;
        }

        private void OnDisable()
        {
            _gameInput.Player.Dodge.performed -= OnDodging;
            _gameInput.Player.Move.performed -= OnMoving;
            _gameInput.Player.Move.canceled -= OnStaying;
        }

        private void Update() => _currentState.Update();

        private void OnStaying([CanBeNull]InputAction.CallbackContext ctx)
        {
            _rawDirection = Vector3.zero;
            
            if (_dodge.IsProcessing)
                return;

            _currentState = _stayState;
            Staying?.Invoke();
        }

        private void OnMoving(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            Debug.Log($"Input: {input}");
            _rawDirection = new Vector3(input.x, 0f, input.y);
            
            if (_currentState == _moveState || _dodge.IsProcessing)
                return;

            SwitchToMoveState();
        }

        private void OnDodging(InputAction.CallbackContext ctx)
        {
            if (_currentState == _dodgeState)
                return;
            
            if (IsMoving)
                _dodge.Init(_rollingDistance, Direction);
            else
                _dodge.Init(_stepBackDistance, -_model.forward);

            _dodge.Reset();
            _currentState = _dodgeState;
            
            Dodging?.Invoke();
        }
        
        private void OnDodgeCompleted()
        {
            Debug.Log($"Dodge completed. Character moving: {IsMoving}");
            if (IsMoving)
                SwitchToMoveState();
            else
                OnStaying(new InputAction.CallbackContext());
        }

        private void SwitchToMoveState()
        {
            _currentState = _moveState;
            Moving?.Invoke();
        }
    }
}