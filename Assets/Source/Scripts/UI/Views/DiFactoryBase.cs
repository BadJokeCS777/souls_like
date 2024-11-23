using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Loxodon.Framework.ViewModels;
using SL.Services;
using Zenject;

namespace SL.UI.Views
{
    public abstract class DiFactoryBase
    {
        private readonly DiContainer _container;

        protected DiFactoryBase(DiContainer container)
        {
            _container = container;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual T CreateInternal<T>() => _container.Instantiate<T>();

        protected virtual T CreateInternalWithParams<T>(IEnumerable<object> args) => _container.Instantiate<T>(args);
    }

    public class ViewModelsFactory : DiFactoryBase, IViewModelsFactory
    {
        public ViewModelsFactory(DiContainer container) : base(container)
        {
        }

        public T Create<T>() where T : IViewModel => CreateInternal<T>();
        public T CreateWithParams<T>(params object[] args) where T : IViewModel => CreateInternalWithParams<T>(args);
    }
}