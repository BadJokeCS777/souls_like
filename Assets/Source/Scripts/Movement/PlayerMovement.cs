using System;
using SL.Input;
using SL.Movement.States;
using SL.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SL.Movement
{
    public class PlayerMovement : MonoBehaviour, IMovement
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _model;
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private float _speed = 3f;
        [SerializeField, Range(0.01f, 1f)] private float _rotationSpeedRatio = 0.5f;
        [SerializeField] private float _rollingDistance = 3.5f;
        [SerializeField] private float _stepBackDistance = 1f;
        [SerializeField] private float _jumpHeight = 1f;

        private GameInput _gameInput;
        private Transform _camera;
        private Vector3 _rawDirection;
        private Dodge _dodge;

        private State _currentState;
        private StayState _stayState;
        private MoveState _moveState;
        private DodgeState _dodgeState;
        private JumpState _jumpState;

        public event Action Moving;
        public event Action Staying;
        public event Action Dodging;
        public event Action Dodged;
        public event Action Jumping;
        public event Action Jumped;

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

        private bool CantDodge => _jumpState.OnGround == false || _currentState == _dodgeState || _currentState == _jumpState;

        private void Awake()
        {
            _gameInput = new GameInput();
            _dodge = new Dodge();

            _stayState = new StayState();
            _moveState = new MoveState(_speed, _rotationSpeedRatio, transform, _model, this);
            _dodgeState = new DodgeState(_speed, transform, _dodge, OnDodgeCompleted);
            _jumpState = new JumpState(_jumpHeight, _rigidbody, _groundCheckPoint, OnJumpCompleted);


            SetState(_stayState);
        }

        private void OnEnable()
        {
            _gameInput.Enable();
            _gameInput.Player.Dodge.performed += OnDodging;
            _gameInput.Player.Move.performed += OnMoving;
            _gameInput.Player.Move.canceled += OnStaying;
            _gameInput.Player.Jump.canceled += OnJumping;
        }

        private void OnDisable()
        {
            _gameInput.Player.Dodge.performed -= OnDodging;
            _gameInput.Player.Move.performed -= OnMoving;
            _gameInput.Player.Move.canceled -= OnStaying;
            _gameInput.Player.Jump.canceled -= OnJumping;
        }

        private void Update() => _currentState.Update();

        public void Init(Transform cameraTransform)
        {
            _camera = cameraTransform;
        }

        private void OnStaying(InputAction.CallbackContext ctx)
        {
            _rawDirection = Vector3.zero;

            if (_currentState == _jumpState || _dodge.IsProcessing)
                return;

            SetState(_stayState);
            Staying?.Invoke();
        }

        private void OnMoving(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            _rawDirection = new Vector3(input.x, 0f, input.y);

            if (_currentState == _jumpState || _currentState == _moveState || _dodge.IsProcessing)
                return;

            SwitchToMoveState();
        }

        private void OnDodging(InputAction.CallbackContext ctx)
        {
            if (CantDodge)
                return;

            if (IsMoving)
                _dodge.Init(_rollingDistance, Direction);
            else
                _dodge.Init(_stepBackDistance, -_model.forward);

            _dodge.Reset();
            _currentState = _dodgeState;

            Dodging?.Invoke();
        }

        private void OnJumping(InputAction.CallbackContext ctx)
        {
            if (_jumpState.OnGround == false)
                return;

            if (_currentState == _jumpState)
                return;

            SetState(_jumpState);
            Jumping?.Invoke();
        }

        private void OnDodgeCompleted()
        {
            Dodged?.Invoke();
            OnActionEnd();
        }

        private void OnJumpCompleted()
        {
            Jumped?.Invoke();
            OnActionEnd();
        }

        private void OnActionEnd()
        {
            if (IsMoving)
                SwitchToMoveState();
            else
                OnStaying(new InputAction.CallbackContext());
        }

        private void SwitchToMoveState()
        {
            SetState(_moveState);
            Moving?.Invoke();
        }

        private void SetState(State state)
        {
            _currentState = state;
            _currentState.Begin();
        }

        #if UNITY_EDITOR
        [ContextMenu(nameof(Jump))]
        private void Jump()
        {
            _jumpState.Jump();
        }
        #endif
    }
}