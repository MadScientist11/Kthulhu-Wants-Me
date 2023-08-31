using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.EntryPoints
{
    public class Boot : IAsyncStartable
    {
        private readonly IReadOnlyList<IInitializableService> _services;
        private readonly ISceneLoader _sceneLoader;
        private readonly AppLifetimeScope _appLifetimeScope;

        public Boot(AppLifetimeScope appLifetimeScope, IReadOnlyList<IInitializableService> services,
            ISceneLoader sceneLoader)
        {
            _appLifetimeScope = appLifetimeScope;
            _sceneLoader = sceneLoader;
            _services = services;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            List<UniTask> initializationTasks = Enumerable.Select(_services, service =>
            {
                service.IsInitialized = true;
                return service.Initialize();
            }).ToList();
            await UniTask.WhenAll(initializationTasks);
            await _sceneLoader
                .LoadSceneInjected(GameConstants.Scenes.GamePath, LoadSceneMode.Additive, _appLifetimeScope);
        }
    }
}