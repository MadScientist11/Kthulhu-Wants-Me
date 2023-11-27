﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.UI.MainMenu.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.EntryPoints
{
    public class Boot : IAsyncStartable
    {
        private readonly IReadOnlyList<IInitializableService> _services;
        private readonly ISceneLoader _sceneLoader;
        private readonly AppLifetimeScope _appLifetimeScope;
        private readonly IDataProvider _dataProvider;
        private SettingsService _settingsService;

        public Boot(AppLifetimeScope appLifetimeScope, IReadOnlyList<IInitializableService> services,
            ISceneLoader sceneLoader, IDataProvider dataProvider, SettingsService settingsService)
        {
            _settingsService = settingsService;
            _dataProvider = dataProvider;
            _appLifetimeScope = appLifetimeScope;
            _sceneLoader = sceneLoader;
            _services = services;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _dataProvider.Initialize();
            await _settingsService.Initialize();
            List<UniTask> initializationTasks = Enumerable.Select(_services, service =>
            {
                service.IsInitialized = true;
                return service.Initialize();
            }).ToList();
            
            await UniTask.WhenAll(initializationTasks);
            
            await _sceneLoader
                .LoadSceneInjected("MainMenu", LoadSceneMode.Additive, _appLifetimeScope);
        }
    }
}