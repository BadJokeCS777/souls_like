using System.Collections;
using SL.Movement;
using UnityEngine;

namespace SL.General.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private const float HeavyRoll = 2f;
        private const float LightRoll = 1f;
        private const float StepBack = 0f;
        private const float MovingSpeed = 1f;

        private static readonly int Speed = Animator.StringToHash(nameof(Speed));
        private static readonly int DodgeType = Animator.StringToHash(nameof(DodgeType));
        private static readonly int Dodge = Animator.StringToHash(nameof(Dodge));
        private static readonly int Jump = Animator.StringToHash(nameof(Jump));
        private static readonly int Sitting = Animator.StringToHash(nameof(Sitting));

        [SerializeField, Min(0f)] private float _weight = 0.25f;
        [SerializeField, Min(0f)] private float _movingChangeDuration = 0.1f;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Animator _animator;

        private float _speed = 0f;
        private Coroutine _speedChanging;

        public Animator Animator => _animator;

        private float DodgeValue
        {
            get
            {
                if (_playerMovement.IsMoving)
                    return _weight > 0.5f ? HeavyRoll : LightRoll;

                return StepBack;
            }
        }

        private void OnEnable()
        {
            _playerMovement.Moving += OnMoving;
            _playerMovement.Staying += OnStaying;
            _playerMovement.Dodging += OnDodging;
            _playerMovement.Dodged += OnDodged;
            _playerMovement.Jumping += OnJumping;
            _playerMovement.Jumped += OnJumped;
            _playerMovement.Sitting += OnSitting;
            _playerMovement.Sitted += OnSitted;
        }

        private void OnDisable()
        {
            _playerMovement.Moving -= OnMoving;
            _playerMovement.Staying -= OnStaying;
            _playerMovement.Dodging -= OnDodging;
            _playerMovement.Dodged -= OnDodged;
            _playerMovement.Jumping -= OnJumping;
            _playerMovement.Jumped -= OnJumped;
            _playerMovement.Sitting -= OnSitting;
            _playerMovement.Sitted -= OnSitted;
        }

        private void OnMoving() => StartChangeSpeed(MovingSpeed);

        private void OnStaying() => StartChangeSpeed(0f);

        private void StartChangeSpeed(float newValue)
        {
            if (_speedChanging != null)
                StopCoroutine(_speedChanging);

            StartCoroutine(SpeedChanging(newValue));
        }

        private IEnumerator SpeedChanging(float targetValue)
        {
            float maxDelta = 0f;
            float startValue = _speed;

            while (Mathf.Approximately(_speed, targetValue) == false)
            {
                yield return null;

                maxDelta += Time.deltaTime / _movingChangeDuration;
                _speed = Mathf.MoveTowards(startValue, targetValue, maxDelta);
                _animator.SetFloat(Speed, _speed);
            }
        }

        private void OnDodging()
        {
            _animator.SetFloat(DodgeType, DodgeValue);
            _animator.SetTrigger(Dodge);
        }

        private void OnDodged() => _animator.SetBool(Dodge, false);

        private void OnJumping() => _animator.SetTrigger(Jump);
        private void OnJumped() => _animator.ResetTrigger(Jump);
        private void OnSitting() => _animator.SetBool(Sitting, true);
        private void OnSitted() => _animator.SetBool(Sitting, false);
    }
}