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
        public string ButtonText { get; set; }
        public Sprite ButtonIcon { get; set; }

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
            ButtonText = message.ButtonText;
            ButtonIcon = message.ButtonIcon;
        }

        private void OnHideInteractionMessage(HideInteractionMessage message)
        {
            Active = false;
        }
    }
}