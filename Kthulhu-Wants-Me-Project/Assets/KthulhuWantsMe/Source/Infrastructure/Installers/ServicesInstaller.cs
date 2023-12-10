using KthulhuWantsMe.Source.Gameplay.InGameConsole;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.Audio;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.UI.MainMenu.Settings;
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
                .Register<SceneService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            builder
                .Register<InputService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            builder
                .Register<DataProvider>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder
                .Register<PauseService>(Lifetime.Scoped)
                .AsImplementedInterfaces();

            builder
                .Register<SettingsService>(Lifetime.Singleton)
                .AsSelf();
            
            builder
                .RegisterComponentOnNewGameObject<InGameConsoleService>(Lifetime.Singleton, "InGameConsoleService")
                .AsSelf();

            builder
                .Register<BackgroundMusicPlayer>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder
                .Register<AudioService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
   
            builder
                .Register<ProgressService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder
                .Register<UIFactory>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            
            builder
                .Register<UIService>(Lifetime.Singleton)
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