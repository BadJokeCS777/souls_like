using System;
using BindingProxy;
using PropertyChanged;
using SL.Health.Models;
using Zenject;

namespace SL.Health.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class HealthViewModel : IInitializable, IDisposable
    {
        private readonly HealthModel _model;

        public HealthViewModel(HealthModel model)
        {
            _model = model;
        }

        public float FillAmount { get; set; }

        public void Initialize()
        {
            _model.StateChanged += OnStateChanged;
            OnStateChanged();
        }

        public void Dispose() => _model.StateChanged -= OnStateChanged;

        private void OnStateChanged() => FillAmount = _model.Value / _model.MaxValue;
    }
}