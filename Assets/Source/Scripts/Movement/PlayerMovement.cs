using System;
using SL.Common;
using SL.Input;
using SL.Movement.States;
using SL.Signals;
using SL.States;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SL.Movement
{
    public class PlayerMovement : MessengerBehaviour
    {
        [SerializeField] private Transform _model;
        [SerializeField] private float _speed = 3f;
        [SerializeField, Range(0.01f, 1f)] private float _rotationSpeedRatio = 0.5f;
        [SerializeField] private CharacterController _characterController;

        [Header("Rolls/Dodge")]
        [SerializeField] private float _rollingDistance = 3.5f;
        [SerializeField] private float _stepBackDistance = 1f;

        [Header("Jump")]
        [SerializeField] private bool _isJumping;
        [SerializeField] private float _groundedGravity = -0.05f;
        [SerializeField] private float _jumpHeight = 1f;
        [SerializeField] private float _jumpTime = 1f;
        [SerializeField] private float _fallMultiplier = 2f;

        private float _gravity = -9.8f;
        private float _initialJumpVelocity = 1f;

        private GameInput _gameInput;
        private Transform _camera;
        private Vector3 _rawDirection;
        private Vector3 _direction;
        private Vector3 _movement;
        private Dodge _dodge;

        private State _currentState;
        private DodgeState _dodgeState;

        private bool _isMovementPressed;
        private bool _isJumpPressed;

        public event Action Moving;
        public event Action Staying;
        public event Action Dodging;
        public event Action Dodged;
        public event Action NotGrounded;
        public event Action Grounded;
        public event Action Sitting;
        public event Action Standing;

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

        private bool CantDodge => _characterController.isGrounded == false || _currentState == _dodgeState;

        [Inject]
        private void Construct([Inject(Id = InjectionsConsts.CameraTransformId)]Transform cameraTransform)
        {
            _camera = cameraTransform;
            _gameInput = new GameInput();
            _dodge = new Dodge();
            _dodgeState = new DodgeState(_speed, transform, _dodge, OnDodgeCompleted);

            SetupJumpVariables();
            Subscribe<EnableMovementMessage>(OnEnableMovementMessage);
        }

        private void OnEnable()
        {
            _gameInput.Enable();

            _gameInput.Player.Jump.started += OnJumping;
            _gameInput.Player.Jump.canceled += OnJumping;

            _gameInput.Player.Dodge.performed += OnDodging;
        }

        private void OnDisable()
        {
            _gameInput.Disable();

            _gameInput.Player.Jump.started -= OnJumping;
            _gameInput.Player.Jump.canceled -= OnJumping;

            _gameInput.Player.Dodge.performed -= OnDodging;
        }

        private void Update()
        {
            HandleMovementInput();
            HandleRotation();

            _characterController.Move(_speed * Time.deltaTime * _movement);

            HandleGravity();
            HandleJump();
            HandleGrounded();
        }

        public void SitDown()
        {
            Sitting?.Invoke();
            enabled = false;
        }

        public void StandUp() => Standing?.Invoke();

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

        private void OnJumping(InputAction.CallbackContext ctx) => _isJumpPressed = ctx.ReadValueAsButton();

        private void OnDodgeCompleted() => Dodged?.Invoke();

        private void OnEnableMovementMessage(EnableMovementMessage message) => enabled = true;

        private void HandleMovementInput()
        {
            var input = _gameInput.Player.Move.ReadValue<Vector2>();
            _rawDirection = new Vector3(input.x, 0f, input.y);
            _isMovementPressed = input.sqrMagnitude > 0f;

            _direction = Direction;
            _movement.x = _direction.x;
            _movement.z = _direction.z;

            if (_isMovementPressed)
                Moving?.Invoke();
            else
                Staying?.Invoke();
        }

        private void HandleRotation()
        {
            if (_isMovementPressed == false)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            _model.rotation = Quaternion.Slerp(_model.rotation, targetRotation, _rotationSpeedRatio);
        }

        private void HandleGravity()
        {
            bool isFalling = _movement.y <= 0f;
            if (_characterController.isGrounded)
            {
                _movement.y = _groundedGravity;
            }
            else
            {
                float multiplier = isFalling ? _fallMultiplier : 1f;
                float previousYVelocity = _movement.y;
                float newYVelocity = _movement.y + _gravity * multiplier * Time.deltaTime;
                float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
                _movement.y = nextYVelocity;
            }
        }

        private void HandleJump()
        {
            if (_isJumping == false && _characterController.isGrounded && _isJumpPressed)
            {
                _isJumping = true;
                _movement.y = _initialJumpVelocity * 0.5f;
            }
            else if(_isJumpPressed == false && _isJumping && _characterController.isGrounded)
            {
                _isJumping = false;
            }
        }

        private void HandleGrounded()
        {
            if (_characterController.isGrounded)
                Grounded?.Invoke();
            else
                NotGrounded?.Invoke();
        }

        private void SetupJumpVariables()
        {
            float doubleJumpHeight = 2 * _jumpHeight;
            float timeToApex = _jumpTime * 0.5f;
            _gravity = -doubleJumpHeight / Mathf.Pow(timeToApex, 2);
            _initialJumpVelocity = doubleJumpHeight / timeToApex;
        }
    }
}