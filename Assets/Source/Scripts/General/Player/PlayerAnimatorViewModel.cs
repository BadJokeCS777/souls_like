using System.Collections;
using BindingProxy;
using Loxodon.Framework.Asynchronous;
using Loxodon.Framework.Execution;
using PropertyChanged;
using SL.Common;
using UnityEngine;

namespace SL.General.Player
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class PlayerAnimatorViewModel
    {
        private const float HeavyRoll = 2f;
        private const float LightRoll = 1f;
        private const float StepBack = 0f;

        public readonly PlayerAnimatorModel Model;

        private readonly float _weight;
        private readonly float _heavyRollThreshold;
        private readonly float _movingSpeed;
        private readonly float _movingChangeDuration;
        private readonly ICoroutineExecutor _coroutineExecutor;

        private IAsyncResult _speedChanging;

        public PlayerAnimatorViewModel(float weight, float heavyRollThreshold, float movingSpeed, float movingChangeDuration,
            ICoroutineExecutor coroutineExecutor, PlayerAnimatorModel model)
        {
            _weight = weight;
            _heavyRollThreshold = heavyRollThreshold;
            _movingSpeed = movingSpeed;
            _movingChangeDuration = movingChangeDuration;
            _coroutineExecutor = coroutineExecutor;
            Model = model;
            Model.IsMovingChanged += OnIsMovingChanged;
            Model.IsDodgingChanged += OnIsDodgingChanged;
        }

        public float Speed { get; set; }
        public bool IsDodging { get; set; }

        private float DodgeValue =>
            Model.IsMoving
                ? _weight > _heavyRollThreshold
                    ? HeavyRoll
                    : LightRoll
                : StepBack;

        private void StartChangeSpeed(float newValue)
        {
            _speedChanging?.Cancel();
            _speedChanging = _coroutineExecutor.RunOnCoroutine(SpeedChanging(newValue));
        }

        private IEnumerator SpeedChanging(float targetValue)
        {
            float maxDelta = 0f;
            float startValue = Speed;

            while (Mathf.Approximately(Speed, targetValue) == false)
            {
                yield return null;

                maxDelta += Time.deltaTime / _movingChangeDuration;
                Speed = Mathf.MoveTowards(startValue, targetValue, maxDelta);
            }
        }

        private void OnIsMovingChanged() => StartChangeSpeed(Model.IsMoving ? _movingSpeed : 0f);

        private void OnIsDodgingChanged()
        {
            Model.DodgeValue = DodgeValue;
            IsDodging = Model.IsDodging;
        }
    }
}