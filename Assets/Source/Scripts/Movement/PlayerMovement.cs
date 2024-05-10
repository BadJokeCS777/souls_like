using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField, Range(0.01f, 1f)] private float _rotationSpeedRatio = 0.5f;
        [SerializeField] private Transform _model;
        [SerializeField] private float _rollingDistance = 3.5f;
        [SerializeField] private float _stepBackDistance = 1f;

        private float _dodgeDistance;
        private float _dodgeDistanceProgress;
        private Vector3 _dodgeDirection;
        private MovementState _state = MovementState.Stay;

        private GameInput _gameInput;
        private Transform _camera;
        private Vector3 _direction;

        public event Action Moving;
        public event Action Staying;
        public event Action Dodging;

        public bool IsMoving => _direction.sqrMagnitude > 0f;
        private bool IsDodgeProcessing => _dodgeDistanceProgress < _dodgeDistance;
        
        private Vector3 Direction
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

        private void OnStaying(InputAction.CallbackContext ctx)
        {
            _direction = Vector3.zero;
            
            if (IsDodgeProcessing)
                return;

            _state = MovementState.Stay;
            Staying?.Invoke();
        }

        private void OnMoving(InputAction.CallbackContext ctx)
        {
            Vector2 rawDirection = _gameInput.Player.Move.ReadValue<Vector2>();
            _direction = new Vector3(rawDirection.x, 0f, rawDirection.y);
            
            if (_state == MovementState.Move || IsDodgeProcessing)
                return;

            _state = MovementState.Move;
            Moving?.Invoke();
        }

        private void OnDodging(InputAction.CallbackContext ctx)
        {
            if (_state == MovementState.Dodge)
                return;

            if (_direction.sqrMagnitude > 0f)
                StartDodging(_rollingDistance, Direction);
            else
                StartDodging(_stepBackDistance, -_model.forward);

            _dodgeDistanceProgress = 0f;
            _state = MovementState.Dodge;
        }
    
        private void StartDodging(float distance, Vector3 direction)
        {
            _dodgeDistance = distance;
            _dodgeDirection = direction;
            
            Dodging?.Invoke();
        }

        private void SelfMoving()
        {
            if (!(_direction.sqrMagnitude > 0f))
                return;
            
            Vector3 direction = Direction;
            
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
                _state = _direction.sqrMagnitude > 0f ? MovementState.Move : MovementState.Stay;

                if (_direction.sqrMagnitude > 0f)
                {
                    _state = MovementState.Move;
                    OnMoving(new InputAction.CallbackContext());
                }
                else
                {
                    _state = MovementState.Stay;
                    OnStaying(new InputAction.CallbackContext());
                }
            }
        }
    }
}