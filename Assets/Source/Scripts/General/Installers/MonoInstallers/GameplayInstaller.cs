using Loxodon.Framework.Binding;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Messaging;
using SL.Health.Models;
using SL.Interactions;
using Zenject;
using Context = Loxodon.Framework.Contexts.Context;

namespace SL.General.Installers.MonoInstallers
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BundleSetInitialization();
            Container.Bind<IMessenger>().To<Messenger>().AsSingle().NonLazy();
            Container.Bind<HealthModel>().AsSingle();
            Container.Bind<InteractionsModel>().AsSingle();
        }

        private static void BundleSetInitialization()
        {
            ApplicationContext context = Context.GetApplicationContext();
            BindingServiceBundle bindingService = new(context.GetContainer());
            bindingService.Start();
        }
    }
}