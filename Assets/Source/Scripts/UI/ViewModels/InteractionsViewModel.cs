using System.Collections;
using BindingProxy;
using Loxodon.Framework.Execution;
using Loxodon.Framework.Messaging;
using PropertyChanged;
using SL.Interactions;
using SL.Signals;
using UnityEngine;

namespace SL.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    public class InteractionsViewModel : ViewModelBase
    {
        private readonly ICoroutineExecutor _coroutineExecutor;

        private bool _active;
        private InteractionTrigger _trigger;

        public InteractionsViewModel(IMessenger messenger, ICoroutineExecutor coroutineExecutor) : base(messenger)
        {
            _coroutineExecutor = coroutineExecutor;
        }

        public bool Active { get; set; }
        public string Text { get; set; }
        public string ButtonText { get; set; }
        public Sprite ButtonIcon { get; set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _active = false;
            Subscribe<ShowInteractionMessage>(OnShowInteractionMessage);
            Subscribe<HideInteractionMessage>(OnHideInteractionMessage);
        }

        private void OnShowInteractionMessage(ShowInteractionMessage message)
        {
            _trigger = message.Trigger;
            Text = message.Text;
            ButtonText = message.ButtonText;
            ButtonIcon = message.ButtonIcon;
            _active = true;
            _coroutineExecutor.RunOnCoroutineNoReturn(Updating());
        }

        private void OnHideInteractionMessage(HideInteractionMessage message)
        {
            _trigger = null;
            _active = false;
        }

        private IEnumerator Updating()
        {
            while (_active)
            {
                if (_trigger == null)
                    Active = false;
                else
                    Active = _active && _trigger.IsInteractorLooking;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}