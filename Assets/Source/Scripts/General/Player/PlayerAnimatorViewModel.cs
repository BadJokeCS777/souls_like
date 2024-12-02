using System;
using System.Collections;
using System.ComponentModel;
using BindingProxy;
using Loxodon.Framework.Execution;
using PropertyChanged;
using SL.Common;
using UnityEngine;
using IAsyncResult = Loxodon.Framework.Asynchronous.IAsyncResult;

namespace SL.General.Player
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class PlayerAnimatorViewModel : IDisposable
    {
        private const float HeavyRoll = 2f;
        private const float LightRoll = 1f;
        private const float StepBack = 0f;

        public readonly PlayerAnimatorModel Model;

        private readonly AnimationsSettings _settings;
        private readonly ICoroutineExecutor _coroutineExecutor;

        private IAsyncResult _speedChanging;

        public PlayerAnimatorViewModel(AnimationsSettings settings, ICoroutineExecutor coroutineExecutor, PlayerAnimatorModel model)
        {
            _settings = settings;
            _coroutineExecutor = coroutineExecutor;
            Model = model;

            Model.PropertyChanged += OnModelPropertyChanged;
        }

        public float Speed { get; set; }
        public bool IsDodging { get; set; }

        private float SpeedValue =>
            Model.IsRunning
                ? _settings.RunningSpeed
                : Model.IsMoving
                    ? _settings.MovingSpeed
                    : _settings.StayingSpeed;
        private float DodgeValue =>
            Model.IsMoving
                ? _settings.Weight > _settings.HeavyRollThreshold
                    ? HeavyRoll
                    : LightRoll
                : StepBack;

        public void Dispose() => Model.PropertyChanged -= OnModelPropertyChanged;

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

                maxDelta += Time.deltaTime / _settings.MovingChangeDuration;
                Speed = Mathf.MoveTowards(startValue, targetValue, maxDelta);
            }
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsMoving":
                    OnIsMovingChanged();
                    break;
                case "IsRunning":
                    OnIsMovingChanged();
                    break;
                case "IsDodging":
                    OnIsDodgingChanged();
                    break;
            }
        }

        private void OnIsMovingChanged() => StartChangeSpeed(SpeedValue);

        private void OnIsDodgingChanged()
        {
            Model.DodgeValue = DodgeValue;
            IsDodging = Model.IsDodging;
        }
    }
}