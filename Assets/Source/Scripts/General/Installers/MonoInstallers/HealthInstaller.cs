using Loxodon.Framework.Binding;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Messaging;
using SL.Health.Models;
using Zenject;
using Context = Loxodon.Framework.Contexts.Context;

namespace SL.General.Installers.MonoInstallers
{
    public class HealthInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BundleSetInitialization();
            Container.Bind<IMessenger>().To<Messenger>().AsSingle();
            Container.Bind<HealthModel>().AsSingle();
        }

        private static void BundleSetInitialization()
        {
            ApplicationContext context = Context.GetApplicationContext();
            BindingServiceBundle bindingService = new(context.GetContainer());
            bindingService.Start();
        }
    }
}