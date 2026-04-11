using System.ComponentModel;
using BindingProxy;
using Loxodon.Framework.Messaging;
using PropertyChanged;
using SL.Signals;
using SL.UI.Models;
using SL.UI.ViewModels.Base;

namespace SL.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class HealthViewModel : ViewModelBase
    {
        private readonly HealthModel _model;

        public HealthViewModel(HealthModel model, IMessenger messenger) : base(messenger)
        {
            _model = model;
        }

        public float FillAmount { get; set; }

        protected override void OnInitialize()
        {
            _model.PropertyChanged += OnModelPropertyChanged;
            OnStateChanged();
            Subscribe<RestorePlayerMessage>(OnRestorePlayerMessage);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing == false)
                return;
            _model.PropertyChanged -= OnModelPropertyChanged;
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_model.Value):
                case nameof(_model.MaxValue):
                    OnStateChanged();
                    break;
            }
        }

        private void OnStateChanged()
        {
            FillAmount = _model.Value / _model.MaxValue;

            if (FillAmount <= 0f)
                Publish(new ZeroHealthMessage());
        }

        private void OnRestorePlayerMessage() => _model.Value = _model.MaxValue;
    }
}