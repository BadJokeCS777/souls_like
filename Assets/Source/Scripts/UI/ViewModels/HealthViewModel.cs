using BindingProxy;
using Loxodon.Framework.Messaging;
using PropertyChanged;
using SL.Signals;
using SL.UI.Models;

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
            _model.StateChanged += OnStateChanged;
            Subscribe<RestorePlayerMessage>(OnRestorePlayerMessage);
            OnStateChanged();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing == false)
                return;
            _model.StateChanged -= OnStateChanged;
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