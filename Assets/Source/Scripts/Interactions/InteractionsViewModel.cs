using System;
using System.Collections.Generic;
using BindingProxy;
using Loxodon.Framework.Messaging;
using PropertyChanged;
using SL.Signals;
using UnityEngine;
using Zenject;

namespace SL.Interactions
{
    [AddINotifyPropertyChangedInterface]
    [GenerateFieldProxy]
    [GeneratePropertyProxy]
    internal class InteractionsViewModel : IInitializable, IDisposable
    {
        private readonly InteractionsModel _model;
        private readonly IMessenger _messenger;
        private readonly List<IDisposable> _messages = new();

        public InteractionsViewModel(InteractionsModel model, IMessenger messenger)
        {
            _model = model;
            _messenger = messenger;
        }

        public bool Active { get; set; }
        public string Text { get; set; }
        public Sprite Icon { get; set; }

        public void Initialize()
        {
            Active = false;
            _messages.Add(_messenger.Subscribe<ShowInteractionMessage>(OnShowInteractionMessage));
            _messages.Add(_messenger.Subscribe<HideInteractionMessage>(OnHideInteractionMessage));
        }

        public void Dispose()
        {
            foreach (IDisposable message in _messages)
                message.Dispose();
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