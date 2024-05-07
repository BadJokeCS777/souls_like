using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _weight = 0.25f;
        [SerializeField] private PlayerMovement _playerMovement;

        private Animator _animator;
    
        private static readonly int EquipsWeight = Animator.StringToHash(nameof(EquipsWeight));
        private static readonly int IsDodging = Animator.StringToHash(nameof(IsDodging));
        private static readonly int IsMoving = Animator.StringToHash(nameof(IsMoving));

        private void OnValidate()
        {
            if (_animator != null)
                _animator.SetFloat(EquipsWeight, _weight);
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

        private void Start() => _animator = GetComponent<Animator>();

        private void OnMoving() => _animator.SetBool(IsMoving, true);

        private void OnStaying() => _animator.SetBool(IsMoving, false);

        private void OnDodging()
        {
            Debug.Log("Animator Dodge");
            _animator.SetBool(IsDodging, true);
        }

        #region Animation
        private void OnDodged() => _animator.SetBool(IsDodging, false);
        #endregion
    }
}