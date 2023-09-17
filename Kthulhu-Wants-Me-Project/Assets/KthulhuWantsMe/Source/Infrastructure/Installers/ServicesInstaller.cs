using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder
                .Register<ResourcesManager>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            builder
                .Register<SceneLoader>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            builder
                .Register<InputService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            builder
                .Register<DataProvider>(Lifetime.Singleton)
                .AsImplementedInterfaces();
   
            
            builder
                .Register<RandomService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
          
            builder
                .RegisterComponentOnNewGameObject<CoroutineRunner>(Lifetime.Singleton, "CoroutineRunner")
                .AsImplementedInterfaces();
        }
    }
}