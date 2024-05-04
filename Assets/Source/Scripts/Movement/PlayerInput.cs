using System;
using UnityEngine;

namespace Movement
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private float _dodgePressingTime = 0.25f;

        private bool _isJumpPressed = false;
        private float _dodgePressedTime = 0f;
        private Vector3 _direction = Vector3.zero;

        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";
        private const string DodgeAxis = "Jump";

        public event Action Moving;
        public event Action Staying;
        public event Action Dodging;

        public Vector3 Direction => _direction;
        private bool IsDodgeEnabled => _dodgePressedTime > 0f && _dodgePressedTime < _dodgePressingTime;
    
        private void Update()
        {
            _direction.x = Input.GetAxis(HorizontalAxis);
            _direction.z = Input.GetAxis(VerticalAxis);

            if (Input.GetAxis(DodgeAxis) > 0f)
            {
                _dodgePressedTime += Time.deltaTime;

                if (_isJumpPressed == false)
                {
                    _isJumpPressed = true;
                    _dodgePressedTime = 0f;
                }
            }
            else
            {
                _isJumpPressed = false;
            }
        
            if (_direction.sqrMagnitude > 0f)
            {
                if (_isJumpPressed == false && IsDodgeEnabled)
                {
                    _dodgePressedTime = 0f;
                    Dodging?.Invoke();
                    return;
                }
            
                Moving?.Invoke();
                return;
            }
        
            if (_isJumpPressed && _dodgePressedTime < _dodgePressingTime)
            {
                Dodging?.Invoke();
                return;
            }

            Staying?.Invoke();
        }
    }
}