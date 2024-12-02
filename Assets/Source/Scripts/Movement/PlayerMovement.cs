using SL.Common;
using SL.Input;
using SL.Signals;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace SL.Movement
{
    public class PlayerMovement : MessengerBehaviour
    {
        [SerializeField] private Transform _model;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _rotationSpeedRatio = 15f;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;

        //TODO: move it to settings asset?
        [Header("Dodge")]
        [SerializeField] private float _rollingDistance = 3.5f;
        [SerializeField] private float _stepBackDistance = 1f;
        [SerializeField] private AnimationCurve _stepBackCurve;
        [SerializeField] private AnimationCurve _roleCurve;

        [Header("Jump")]
        [SerializeField] private float _groundedGravity = -9.8f;
        [SerializeField] private float _jumpHeight = 1f;
        [SerializeField] private float _jumpTime = 1f;
        [SerializeField] private float _fallMultiplier = 2f;

        private Transform _camera;
        private PlayerAnimatorModel _animatorModel;
        private GameInput _gameInput;

        //Jump
        private float _gravity = -9.8f;
        private float _initialJumpVelocity = 1f;
        private bool _isJumpPressed;
        private bool _isJumping;

        //Movement
        private bool _isMovementPressed;
        private Vector3 _rawDirection;
        private Vector3 _direction;
        private Vector3 _movement;

        //Dodge
        private Vector3 _dodgeStartPoint;
        private Vector3 _dodgeMovement;
        private AnimationCurve _dodgeCurve;
        private bool _isDodging;
        private float _stepBackDuration;
        private float _lightRollDuration;
        private float _heavyRollDuration;
        private float _dodgeDuration;
        private float _dodgeTimer;
        private float _dodgeDistance;

        private bool CanDodge => _characterController.isGrounded && _animatorModel.IsDodging == false;

        [Inject]
        private void Construct([Inject(Id = InjectionsConsts.CameraTransformId)]Transform cameraTransform,
            PlayerAnimatorModel animatorModel)
        {
            _camera = cameraTransform;
            _animatorModel = animatorModel;

            _gameInput = new GameInput();


            SetupJumpVariables();
            SetupDodgeVariables();

            Subscribe<EnableMovementMessage>(OnEnableMovementMessage);
            Subscribe<FinishDodgeMessage>(OnFinishDodgeMessage);

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
            HandleDodge();
            HandleMovement();
            HandleGravity();
            HandleJump();
            HandleGrounded();
        }

        public void SitDown()
        {
            _animatorModel.IsBonfireSitting = true;
            enabled = false;
        }

        public void StandUp() => _animatorModel.IsBonfireSitting = false;

        private void OnDodging(InputAction.CallbackContext ctx)
        {
            if (CanDodge == false)
                return;

            if (_isMovementPressed)
            {
                _dodgeCurve = _roleCurve;
                _dodgeDistance = _rollingDistance;
                _dodgeMovement = _direction;
                _dodgeDuration = _lightRollDuration;
            }
            else
            {
                _dodgeCurve = _stepBackCurve;
                _dodgeDistance = _stepBackDistance;
                _dodgeMovement = -_model.forward;
                _dodgeDuration = _stepBackDuration;
            }

            _dodgeStartPoint = transform.position;
            _dodgeTimer = 0f;
            _isDodging = true;
            _animatorModel.IsDodging = true;
        }

        private void OnJumping(InputAction.CallbackContext ctx) => _isJumpPressed = ctx.ReadValueAsButton();

        private void OnEnableMovementMessage(EnableMovementMessage message) => enabled = true;

        private void OnFinishDodgeMessage()
        {
            _animatorModel.IsDodging = false;
            _isDodging = false;
        }

        private void HandleMovementInput()
        {
            var input = _gameInput.Player.Move.ReadValue<Vector2>();
            _rawDirection = new Vector3(input.x, 0f, input.y);
            _isMovementPressed = input.sqrMagnitude > 0f;

            _direction = CalculateDirection();
            _movement.x = _direction.x;
            _movement.z = _direction.z;

            _animatorModel.IsMoving = _isMovementPressed;
        }

        private void HandleRotation()
        {
            if (_isMovementPressed == false || _isDodging)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            float t = _rotationSpeedRatio * Time.deltaTime;
            _model.rotation = Quaternion.Slerp(_model.rotation, targetRotation, t);
        }

        private void HandleDodge()
        {
            if (_isDodging == false)
                return;

            float evaluation = _dodgeTimer / _dodgeDuration;
            Vector3 targetPoint = _dodgeStartPoint + (_dodgeDistance * _dodgeCurve.Evaluate(evaluation) * _dodgeMovement);
            _movement = targetPoint - transform.position;
            _movement.y = _characterController.isGrounded ? _groundedGravity : _gravity;
            _dodgeTimer += Time.deltaTime;
        }

        private void HandleMovement()
        {
            if (_isDodging)
                _characterController.Move(_movement);
            else
                _characterController.Move(_speed * Time.deltaTime * _movement);
        }

        private void HandleGravity()
        {
            if (_characterController.isGrounded)
            {
                _movement.y = _groundedGravity;
                return;
            }

            bool isFalling = _movement.y <= 0f;
            float multiplier = isFalling ? _fallMultiplier : 1f;
            float previousYVelocity = _movement.y;
            float newYVelocity = _movement.y + _gravity * multiplier * Time.deltaTime;
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            _movement.y = nextYVelocity;
        }

        private void HandleJump()
        {
            if (_isJumping == false && _characterController.isGrounded && _isJumpPressed && _animatorModel.IsDodging == false)
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
            if(_isDodging == false)
                _animatorModel.IsGrounded = _characterController.isGrounded;
        }

        private void SetupJumpVariables()
        {
            float doubleJumpHeight = 2 * _jumpHeight;
            float timeToApex = _jumpTime * 0.5f;
            _gravity = -doubleJumpHeight / Mathf.Pow(timeToApex, 2);
            _initialJumpVelocity = doubleJumpHeight / timeToApex;
        }

        private void SetupDodgeVariables()
        {
            var clips = _animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                switch (clip.name)
                {
                    case "StepBack":
                        _stepBackDuration = clip.length;
                        break;
                    case "LightRoll":
                        _lightRollDuration = clip.length;
                        break;
                    case "HeavyRoll":
                        _heavyRollDuration = clip.length;
                        break;
                }
            }
        }

        private Vector3 CalculateDirection()
        {
            Vector3 direction = _camera.forward * _rawDirection.z + _camera.right * _rawDirection.x;
            direction.y = 0f;
            return direction.normalized;
        }
    }
}