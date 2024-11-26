using System;
using BindingProxy;
using PropertyChanged;

namespace SL.Common
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class PlayerAnimatorModel
    {
        private bool _isMoving;
        private bool _isDodging;

        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (value == _isMoving)
                    return;

                _isMoving = value;
                IsMovingChanged?.Invoke();
            }
        }

        public bool IsDodging
        {
            get => _isDodging;
            set
            {
                if (value == _isDodging)
                    return;

                _isDodging = value;
                IsDodgingChanged?.Invoke();
            }
        }

        public bool IsGrounded { get; set; } = true;
        public bool IsBonfireSitting { get; set; }

        public float DodgeValue { get; set; } = 0f;

        public event Action IsMovingChanged;
        public event Action IsDodgingChanged;
    }
}