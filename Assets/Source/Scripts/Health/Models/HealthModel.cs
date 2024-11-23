using System;
using BindingProxy;
using Loxodon.Framework.Messaging;
using PropertyChanged;
using SL.Signals;

namespace SL.Health.Models
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class HealthModel
    {
        private readonly IMessenger _messenger;

        private float _value;
        private float _maxValue;

        public event Action StateChanged;

        public HealthModel(IMessenger messenger, float value = 100f)
        {
            _messenger = messenger;
            Value = value;
            MaxValue = value;
        }

        public float MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                StateChanged?.Invoke();
            }
        }

        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                StateChanged?.Invoke();

                if (_value <= 0f)
                    _messenger.Publish(new ZeroHealthMessage());
            }
        }
    }
}