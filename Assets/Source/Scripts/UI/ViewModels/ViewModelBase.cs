using System;
using System.Collections.Generic;
using Loxodon.Framework.Messaging;
using Zenject;

namespace SL.UI.ViewModels
{
    public abstract class ViewModelBase : Loxodon.Framework.ViewModels.ViewModelBase, IInitializable
    {
        private readonly List<IDisposable> _subscriptions = new();

        protected ViewModelBase(IMessenger messenger) : base(messenger) { }

        public void Initialize() => OnInitialize();

        protected virtual void OnInitialize() { }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _subscriptions.ForEach(d => d.Dispose());
        }

        protected ISubscription<T> Subscribe<T>(Action<T> action) => Add(Messenger.Subscribe(action));
        protected ISubscription<T> Subscribe<T>(string channel, Action<T> action) => Add(Messenger.Subscribe(channel, action));
        protected ISubscription<T> Subscribe<T>(Action action) => Add(Messenger.Subscribe<T>(_ => action.Invoke()));
        protected ISubscription<T> Subscribe<T>(string channel, Action action) => Add(Messenger.Subscribe<T>(channel, _ => action.Invoke()));

        protected void Publish<T>(T message) => Messenger.Publish(message);
        protected void Publish<T>(string channel, T message) => Messenger.Publish(channel, message);
        protected void Publish(object message) => Messenger.Publish(message);
        protected void Publish(string channel, object message) => Messenger.Publish(channel, message);

        private ISubscription<T> Add<T>(ISubscription<T> subscription)
        {
            _subscriptions.Add(subscription);
            return subscription;
        }
    }
}