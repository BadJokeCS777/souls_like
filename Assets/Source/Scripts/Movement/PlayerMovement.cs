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
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        [SerializeField] private MovementSettings _settings;

        private Transform _camera;
        private PlayerAnimatorModel _animatorModel;
        private GameInput _gameInput;

        //Movement
        private Vector3 _rawDirection;
        private Vector3 _direction;
        private Vector3 _movement;

        //Jump
        private float _gravity = -9.8f;
        private float _initialJumpVelocity;
        private bool _isJumping;

        //Dodge
        private Vector3 _dodgeStartPoint;
        private Vector3 _dodgeMovement;
        private AnimationCurve _dodgeCurve;
        private float _stepBackDuration;
        private float _lightRollDuration;
        private float _heavyRollDuration;
        private float _dodgeDuration;
        private float _dodgeTimer;
        private float _dodgeDistance;

        private bool CanJump => _isJumping == false && _characterController.isGrounded && _animatorModel.IsDodging == false;
        private bool CanDodge => _characterController.isGrounded && _animatorModel.IsDodging == false;

        private void OnEnable()
        {
            _gameInput.Enable();

            _gameInput.Player.Run.performed += OnRunning;
            _gameInput.Player.Run.canceled += OnRunning;
            _gameInput.Player.Dodge.performed += OnDodging;
            _gameInput.Player.Jump.started += OnJumping;
            _gameInput.Player.Jump.canceled += OnJumping;
        }

        private void OnDisable()
        {
            _gameInput.Disable();

            _gameInput.Player.Run.performed -= OnRunning;
            _gameInput.Player.Run.canceled -= OnRunning;
            _gameInput.Player.Dodge.performed -= OnDodging;
            _gameInput.Player.Jump.started -= OnJumping;
            _gameInput.Player.Jump.canceled -= OnJumping;
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

        #region Initialization
        [Inject]
        private void Construct([Inject(Id = InjectionsConsts.CameraId)]Transform cameraTransform,
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

        private void SetupJumpVariables()
        {
            float doubleJumpHeight = 2 * _settings.JumpHeight;
            float timeToApex = _settings.JumpTime * 0.5f;
            _gravity = -doubleJumpHeight / Mathf.Pow(timeToApex, 2);
            _initialJumpVelocity = doubleJumpHeight / timeToApex * 0.5f;;
        }

        private void SetupDodgeVariables()
        {
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
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
        #endregion

        #region OnInput
        private void OnRunning(InputAction.CallbackContext ctx) => _animatorModel.IsRunning = ctx.ReadValueAsButton();

        private void OnDodging(InputAction.CallbackContext ctx)
        {
            if (CanDodge == false)
                return;

            if (_animatorModel.IsMoving)
                SetUpDodge(_settings.RoleCurve, _settings.RollingDistance, _direction, _lightRollDuration);
            else
                SetUpDodge(_settings.StepBackCurve, _settings.StepBackDistance, -_model.forward, _stepBackDuration);
        }

        private void OnJumping(InputAction.CallbackContext ctx)
        {
            if (CanJump == false)
                return;

            _isJumping = true;
            _movement.y = _initialJumpVelocity;
        }

        #endregion

        #region Messages
        private void OnEnableMovementMessage(EnableMovementMessage message) => enabled = true;

        private void OnFinishDodgeMessage() => _animatorModel.IsDodging = false;
        #endregion

        public void BonfireSitDown(Vector3 bonfirePosition)
        {
            _model.transform.LookAt(bonfirePosition);
            _animatorModel.IsBonfireSitting = true;
            enabled = false;
        }

        public void BonfireStandUp() => _animatorModel.IsBonfireSitting = false;

        private void HandleMovementInput()
        {
            var input = _gameInput.Player.Move.ReadValue<Vector2>();
            _rawDirection = new Vector3(input.x, 0f, input.y);
            _animatorModel.IsMoving = input.sqrMagnitude > 0f;

            _direction = CalculateDirection();
            _movement.x = _direction.x;
            _movement.z = _direction.z;
        }

        private void HandleRotation()
        {
            if (_animatorModel.IsMoving == false || _animatorModel.IsDodging)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(_direction);
            float t = _settings.RotationSpeedRatio * Time.deltaTime;
            _model.rotation = Quaternion.Slerp(_model.rotation, targetRotation, t);
        }

        private void HandleDodge()
        {
            if (_animatorModel.IsDodging == false)
                return;

            float evaluation = _dodgeTimer / _dodgeDuration;
            Vector3 targetPoint = _dodgeStartPoint + (_dodgeDistance * _dodgeCurve.Evaluate(evaluation) * _dodgeMovement);
            Vector3 movement = targetPoint - transform.position;
            _movement.x = movement.x;
            _movement.z = movement.z;

            _dodgeTimer += Time.deltaTime;
        }

        private void HandleMovement()
        {
            float speed = _animatorModel.IsRunning && _characterController.isGrounded
                ? _settings.RunSpeed
                : _settings.Speed;
            if (_animatorModel.IsDodging)
                _characterController.Move(_movement);
            else
                _characterController.Move(speed * Time.deltaTime * _movement);
        }

        private void HandleGravity()
        {
            if (_characterController.isGrounded)
            {
                _movement.y = _settings.GroundedGravity;
                return;
            }

            bool isFalling = _movement.y <= 0f;
            float multiplier = isFalling ? _settings.FallMultiplier : 1f;
            float previousYVelocity = _movement.y;
            float newYVelocity = _movement.y + _gravity * multiplier * Time.deltaTime;
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            _movement.y = nextYVelocity;
        }

        private void HandleJump()
        {
            if(_isJumping && _characterController.isGrounded)
                _isJumping = false;
        }

        private void HandleGrounded() => _animatorModel.IsGrounded = _characterController.isGrounded;

        private void SetUpDodge(AnimationCurve curve, float distance, Vector3 direction, float duration)
        {
            _dodgeCurve = curve;
            _dodgeDistance = distance;
            _dodgeMovement = direction;
            _dodgeDuration = duration;

            _dodgeStartPoint = transform.position;
            _dodgeTimer = 0f;
            _animatorModel.IsDodging = true;
        }

        private Vector3 CalculateDirection()
        {
            Vector3 direction = _camera.forward * _rawDirection.z + _camera.right * _rawDirection.x;
            direction.y = 0f;
            return direction.normalized;
        }
    }
}