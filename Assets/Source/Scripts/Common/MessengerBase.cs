using System;
using System.Collections.Generic;
using Loxodon.Framework.Messaging;
using Zenject;

namespace SL.Common
{
    //TODO: In perfect case use by Zenject, else remember about Dispose
    public abstract class MessengerBase : IInitializable, IDisposable
    {
        private readonly List<IDisposable> _subscriptions = new();
        private readonly IMessenger _messenger;

        protected MessengerBase(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public void Initialize() => OnInitialize();

        public void Dispose()
        {
            foreach (IDisposable subscription in _subscriptions)
                subscription.Dispose();

            OnDispose();
        }

        protected ISubscription<T> Subscribe<T>(Action<T> action) => Add(_messenger.Subscribe(action));
        protected ISubscription<T> Subscribe<T>(string channel, Action<T> action) => Add(_messenger.Subscribe(channel, action));
        protected ISubscription<T> Subscribe<T>(Action action) => Add(_messenger.Subscribe<T>(_ => action.Invoke()));
        protected ISubscription<T> Subscribe<T>(string channel, Action action) => Add(_messenger.Subscribe<T>(channel, _ => action.Invoke()));

        protected void Publish<T>(T message) => _messenger.Publish(message);
        protected void Publish<T>(string channel, T message) => _messenger.Publish(channel, message);
        protected void Publish(object message) => _messenger.Publish(message);
        protected void Publish(string channel, object message) => _messenger.Publish(channel, message);

        protected virtual void OnInitialize() { }
        protected virtual void OnDispose() { }

        private ISubscription<T> Add<T>(ISubscription<T> subscription)
        {
            _subscriptions.Add(subscription);
            return subscription;
        }
    }
}