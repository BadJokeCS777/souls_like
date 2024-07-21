using System.Collections;
using SL.Movement;
using UnityEngine;

namespace SL.General
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

        [SerializeField, Min(0f)] private float _weight = 0.25f;
        [SerializeField, Min(0f)] private float _movingChangeDuration = 0.1f;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Animator _animator;

        private float _speed = 0f;
        private Coroutine _speedChanging;

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
        }

        private void OnDisable()
        {
            _playerMovement.Moving -= OnMoving;
            _playerMovement.Staying -= OnStaying;
            _playerMovement.Dodging -= OnDodging;
            _playerMovement.Dodged -= OnDodged;
        }

        private void OnMoving() => StartChangeSpeed(MovingSpeed);

        private void OnStaying() => StartChangeSpeed(0f);

        private void OnDodging()
        {
            _animator.SetFloat(DodgeType, DodgeValue);
            _animator.SetTrigger(Dodge);
        }

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

        private void OnDodged() => _animator.SetBool(Dodge, false);
    }
}