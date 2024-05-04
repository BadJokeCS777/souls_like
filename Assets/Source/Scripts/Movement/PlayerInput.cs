using System;
using UnityEngine;

namespace Movement
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private float _dodgePressingTime = 0.25f;

        private bool _isDodgePressed = false;
        private float _dodgePressedTime = 0f;
        private Vector3 _direction = Vector3.zero;
        private Transform _camera;

        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";
        private const string DodgeAxis = "Jump";

        public event Action Moving;
        public event Action Staying;
        public event Action Dodging;

        public Vector3 Direction
        {
            get
            {
                Vector3 direction = _camera.forward * _direction.z + _camera.right * _direction.x;
                direction.y = 0f;
                return direction.normalized;
            }
        }
        private bool IsDodgeEnabled => _dodgePressedTime > 0f && _dodgePressedTime < _dodgePressingTime;

        private void Awake() => _camera = Camera.main.transform;

        private void Update()
        {
            _direction.x = Input.GetAxis(HorizontalAxis);
            _direction.z = Input.GetAxis(VerticalAxis);

            if (Input.GetAxis(DodgeAxis) > 0f)
            {
                _dodgePressedTime += Time.deltaTime;

                if (_isDodgePressed == false)
                {
                    _isDodgePressed = true;
                    _dodgePressedTime = 0f;
                }
            }
            else
            {
                _isDodgePressed = false;
            }
        
            if (_direction.sqrMagnitude > 0f)
            {
                if (_isDodgePressed == false && IsDodgeEnabled)
                {
                    _dodgePressedTime = 0f;
                    Dodging?.Invoke();
                    return;
                }
            
                Moving?.Invoke();
                return;
            }
        
            if (_isDodgePressed && _dodgePressedTime < _dodgePressingTime)
            {
                Dodging?.Invoke();
                return;
            }

            Staying?.Invoke();
        }
    }
}