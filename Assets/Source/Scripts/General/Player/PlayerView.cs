using System.Collections;
using Loxodon.Framework.Binding;
using SL.Movement;
using UnityEngine;
using Zenject;

namespace SL.General.Player
{
    public class PlayerView : MonoBehaviour
    {
        private const float HeavyRoll = 2f;
        private const float LightRoll = 1f;
        private const float StepBack = 0f;
        private const float MovingSpeed = 1f;

        private static readonly int Speed = Animator.StringToHash(nameof(Speed));
        private static readonly int DodgeType = Animator.StringToHash(nameof(DodgeType));
        private static readonly int Dodge = Animator.StringToHash(nameof(Dodge));
        private static readonly int Sitting = Animator.StringToHash(nameof(Sitting));
        private static readonly int OnGround = Animator.StringToHash(nameof(OnGround));

        [SerializeField, Min(0f)] private float _weight = 0.25f;
        [SerializeField, Min(0f)] private float _movingChangeDuration = 0.1f;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Animator _animator;

        private float _speed = 0f;
        private Coroutine _speedChanging;
        private PlayerAnimatorViewModel _viewModel;

        private float DodgeValue
        {
            get
            {
                if (_playerMovement.IsMoving)
                    return _weight > 0.5f ? HeavyRoll : LightRoll;

                return StepBack;
            }
        }

        //TODO: finish binding
        // [Inject]
        // private void Construct()
        // {
        //     _viewModel = new PlayerAnimatorViewModel();
        //     Bind();
        // }
        //
        // private void Bind()
        // {
        //     var bindingSet = this.CreateBindingSet(_viewModel);
        //     
        //     bindingSet.Build();
        // }

        private void OnEnable()
        {
            _playerMovement.Moving += OnMoving;
            _playerMovement.Staying += OnStaying;
            _playerMovement.Dodging += OnDodging;
            _playerMovement.Dodged += OnDodged;
            _playerMovement.NotGrounded += OnNotGrounded;
            _playerMovement.Grounded += OnGrounded;
            _playerMovement.Sitting += OnSitting;
            _playerMovement.Standing += OnStanding;
        }

        private void OnDisable()
        {
            _playerMovement.Moving -= OnMoving;
            _playerMovement.Staying -= OnStaying;
            _playerMovement.Dodging -= OnDodging;
            _playerMovement.Dodged -= OnDodged;
            _playerMovement.NotGrounded -= OnNotGrounded;
            _playerMovement.Grounded -= OnGrounded;
            _playerMovement.Sitting -= OnSitting;
            _playerMovement.Standing -= OnStanding;
        }

        private void OnMoving() => StartChangeSpeed(MovingSpeed);
        private void OnStaying() => StartChangeSpeed(0f);
        private void OnDodged() => _animator.SetBool(Dodge, false);
        private void OnSitting() => _animator.SetBool(Sitting, true);
        private void OnStanding() => _animator.SetBool(Sitting, false);

        private void OnNotGrounded() => _animator.SetBool(OnGround, false);
        private void OnGrounded() => _animator.SetBool(OnGround, true);

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
    }
}