using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private const float HeavyDodge = 2f;
        private const float LightDodge = 1f;
        private const float StepBack = 0f;

        private static readonly int Speed = Animator.StringToHash(nameof(Speed));
        private static readonly int DodgeType = Animator.StringToHash(nameof(DodgeType));
        private static readonly int Dodge = Animator.StringToHash(nameof(Dodge));

        [SerializeField, Min(0f)] private float _weight = 0.25f;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Animator _animator;

        private float DodgeValue
        {
            get
            {
                if (_playerMovement.IsMoving)
                    return _weight > 0.5f ? HeavyDodge : LightDodge;
            
                return StepBack;
            }
        }

        private void OnEnable()
        {
            _playerMovement.Moving += OnMoving;
            _playerMovement.Staying += OnStaying;
            _playerMovement.Dodging += OnDodging;
        }

        private void OnDisable()
        {
            _playerMovement.Moving -= OnMoving;
            _playerMovement.Staying -= OnStaying;
            _playerMovement.Dodging -= OnDodging;
        }

        private void OnMoving() => _animator.SetFloat(Speed, 1f);

        private void OnStaying() => _animator.SetFloat(Speed, 0f);

        private void OnDodging()
        {
            _animator.SetFloat(DodgeType, DodgeValue);
            _animator.SetTrigger(Dodge);
        }

        #region Animation
        private void OnDodged() => _animator.SetBool(Dodge, false);
        #endregion
    }
}