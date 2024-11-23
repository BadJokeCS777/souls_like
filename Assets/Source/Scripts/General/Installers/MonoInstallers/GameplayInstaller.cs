using Loxodon.Framework.Binding;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Messaging;
using SL.Services;
using SL.UI.Views;
using SL.UI.Models;
using Zenject;

namespace SL.General.Installers.MonoInstallers
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BundleSetInitialization();
            Container.Bind<IViewModelsFactory>().To<ViewModelsFactory>().AsSingle();
            Container.Bind<IMessenger>().To<Messenger>().AsSingle();
            Container.Bind<HealthModel>().AsSingle();
        }

        private static void BundleSetInitialization()
        {
            ApplicationContext context = Loxodon.Framework.Contexts.Context.GetApplicationContext();
            BindingServiceBundle bindingService = new(context.GetContainer());
            bindingService.Start();
        }
    }
}