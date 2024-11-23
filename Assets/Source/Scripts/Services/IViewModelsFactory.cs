using Loxodon.Framework.ViewModels;

namespace SL.Services
{
    public interface IDiFactory
    {
        T Create<T>();
        T CreateWithParams<T>(params object[] args);
    }

    public interface IDiFactory<in TFilter>
    {
        T Create<T>() where T : TFilter;
        T CreateWithParams<T>(params object[] args) where T : TFilter;
    }

    public interface IViewModelsFactory : IDiFactory<IViewModel>
    {
    }
}