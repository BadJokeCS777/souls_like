using System;
using System.Linq;
using Input;
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

        private Movement _state = Movement.Stay;

        private GameInput _gameInput;
        private Transform _camera;
        private Vector3 _direction;
        private Dodge _dodge;
        private State _currentState;
        private State[] _states;

        public event Action Moving;
        public event Action Staying;
        public event Action Dodging;

        public bool IsMoving => _direction.sqrMagnitude > 0f;
        
        public Vector3 Direction
        {
            get
            {
                Vector3 direction = _camera.forward * _direction.z + _camera.right * _direction.x;
                direction.y = 0f;
                return direction.normalized;
            }
        }
        
        private void Awake()
        {
            _camera = Camera.main.transform;
            _gameInput = new GameInput();
            _dodge = new Dodge();
            _states = new State[]
            {
                new StayState(),
                new MoveState(_speed, _rotationSpeedRatio, transform, _model, this),
                new DodgeState(_speed, transform, _dodge, OnDodgeCompleted)
            };
            
            _currentState = _states[0];
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

        private void OnStaying(InputAction.CallbackContext ctx)
        {
            _direction = Vector3.zero;
            
            if (_dodge.IsProcessing)
                return;

            _state = Movement.Stay;
            _currentState = _states.First(_ => _ is StayState);
            Staying?.Invoke();
        }

        private void OnMoving(InputAction.CallbackContext ctx)
        {
            Vector2 rawDirection = ctx.ReadValue<Vector2>();
            _direction = new Vector3(rawDirection.x, 0f, rawDirection.y);
            
            if (_state == Movement.Move || _dodge.IsProcessing)
                return;

            _state = Movement.Move;
            _currentState = _states.First(_ => _ is MoveState);
            Moving?.Invoke();
        }

        private void OnDodging(InputAction.CallbackContext ctx)
        {
            if (_state == Movement.Dodge)
                return;

            _state = Movement.Dodge;
            
            if (IsMoving)
                _dodge.Init(_rollingDistance, Direction);
            else
                _dodge.Init(_stepBackDistance, -_model.forward);

            _dodge.Reset();
            _currentState = _states.First(_ => _ is DodgeState);
            
            Dodging?.Invoke();
        }
        
        private void OnDodgeCompleted()
        {
            if (IsMoving)
                OnMoving(new InputAction.CallbackContext());
            else
                OnStaying(new InputAction.CallbackContext());
        }
    }
}