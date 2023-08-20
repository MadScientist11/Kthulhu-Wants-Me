using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.EntryPoints
{
    public class Boot : IAsyncStartable
    {
        private readonly IReadOnlyList<IInitializableService> _services;
        private IReadOnlyList<IPreInitializableService> _preInitializableServices;
        private ISceneLoader _sceneLoader;

        public Boot(IReadOnlyList<IInitializableService> services, ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _services = services;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            List<UniTask> initializationTasks = Enumerable.Select(_services, service => service.Initialize()).ToList();
            await UniTask.WhenAll(initializationTasks);
            _sceneLoader.LoadScene(GameConstants.Scenes.GamePath, LoadSceneMode.Single);
        }

     

     
        
    }
}