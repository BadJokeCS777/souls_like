using System;
using BindingProxy;
using PropertyChanged;

namespace SL.UI.Models
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class HealthModel
    {
        private float _value;
        private float _maxValue;

        public event Action StateChanged;

        public HealthModel(float value = 100f)
        {
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
            }
        }
    }
}