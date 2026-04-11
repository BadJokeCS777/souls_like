using Loxodon.Framework.Binding;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Execution;
using Loxodon.Framework.Messaging;
using SL.Common;
using SL.Game.Bonfires;
using SL.Services;
using SL.UI.Models;
using SL.UI.Views.Base;
using UnityEngine;
using Zenject;

namespace SL.Game.Installers.MonoInstallers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private Transform _cameraTransform;

        public override void InstallBindings()
        {
            BundleSetInitialization();

            //Factories
            Container.Bind<IViewModelsFactory>().To<ViewModelsFactory>().AsSingle();
            Container.Bind<IPrefabFactory>().To<Factories.PrefabFactory>().AsSingle();

            //Infrastructure
            Container.Bind<ICoroutineExecutor>().To<CoroutineExecutor>().AsSingle();

            //Commons
            Container.Bind<IMessenger>().To<Messenger>().AsSingle();
            Container.Bind<Transform>().WithId(InjectionsConsts.CameraId).FromInstance(_cameraTransform).AsSingle();

            //Models
            //Container.Bind<List<StatsConfig>>().FromInstance(_statsSettings.Stats).AsSingle();
            Container.Bind<PlayerAnimatorModel>().AsSingle();
            Container.Bind<HealthModel>().AsSingle();
            Container.Bind<StatsModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<StatsStorage>().AsSingle();

            //Services
            Container.BindInterfacesAndSelfTo<BonfireManager>().AsSingle().NonLazy();
        }

        private static void BundleSetInitialization()
        {
            ApplicationContext context = Loxodon.Framework.Contexts.Context.GetApplicationContext();
            BindingServiceBundle bindingService = new(context.GetContainer());
            bindingService.Start();
        }
    }
}