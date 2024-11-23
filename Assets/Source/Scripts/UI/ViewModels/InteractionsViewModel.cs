using BindingProxy;
using Loxodon.Framework.Messaging;
using PropertyChanged;
using SL.Signals;
using UnityEngine;

namespace SL.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class InteractionsViewModel : ViewModelBase
    {
        public InteractionsViewModel(IMessenger messenger) : base(messenger) { }

        public bool Active { get; set; }
        public string Text { get; set; }
        public Sprite Icon { get; set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Active = false;
            Subscribe<ShowInteractionMessage>(OnShowInteractionMessage);
            Subscribe<HideInteractionMessage>(OnHideInteractionMessage);
        }

        private void OnShowInteractionMessage(ShowInteractionMessage message)
        {
            Active = true;
            Text = message.Text;
            Icon = message.Icon;
        }

        private void OnHideInteractionMessage(HideInteractionMessage message)
        {
            Active = false;
        }
    }
}