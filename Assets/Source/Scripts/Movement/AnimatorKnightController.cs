using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorKnightController : MonoBehaviour
    {
        [SerializeField, Range(0f, 1.5f)] private float _weight = 0.5f;
        [SerializeField] private PlayerMovement _playerMovement;

        private Animator _animator;
    
        private static readonly int EquipsWeight = Animator.StringToHash("EquipsWeight");
        private static readonly int IsDodging = Animator.StringToHash("IsDodging");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        private void OnEnable()
        {
            _playerMovement.Moving += OnMoving;
            _playerMovement.Staying += OnStaying;
            _playerMovement.Rolling += OnRolling;
            _playerMovement.StepBacking += OnStepBacking;
        }

        private void OnDisable()
        {
            _playerMovement.Moving -= OnMoving;
            _playerMovement.Staying -= OnStaying;
            _playerMovement.Rolling -= OnRolling;
            _playerMovement.StepBacking -= OnStepBacking;
        }

        private void Start() => _animator = GetComponent<Animator>();

        private void OnMoving() => _animator.SetBool(IsMoving, true);

        private void OnStaying() => _animator.SetBool(IsMoving, false);

        private void OnRolling()
        {
            _animator.SetFloat(EquipsWeight, _weight);
            _animator.SetBool(IsDodging, true);
        }

        private void OnStepBacking() => _animator.SetBool(IsDodging, true);

        private void OnDodged() => _animator.SetBool(IsDodging, false);
    }
}