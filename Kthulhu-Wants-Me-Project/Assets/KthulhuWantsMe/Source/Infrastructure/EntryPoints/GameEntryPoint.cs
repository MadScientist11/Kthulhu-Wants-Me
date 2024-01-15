﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.InGameConsole;
using KthulhuWantsMe.Source.Gameplay.StateMachine;
using KthulhuWantsMe.Source.Gameplay.StateMachine.States;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.EntryPoints
{
    public class GameEntryPoint : IAsyncStartable
    {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly IReadOnlyList<IInitializableService> _services;
        private readonly IUIFactory _uiFactory;
        private readonly IInputService _inputService;
        private readonly GameLifetimeScope _gameLifetimeScope;
        private readonly InGameConsoleService _gameConsoleService;


        public GameEntryPoint(IReadOnlyList<IInitializableService> services, GameplayStateMachine gameplayStateMachine, 
            IUIFactory uiFactory, IInputService inputService, GameLifetimeScope gameLifetimeScope,
            InGameConsoleService gameConsoleService
        )
        {
            _gameConsoleService = gameConsoleService;
            _inputService = inputService;
            _gameLifetimeScope = gameLifetimeScope;
            _uiFactory = uiFactory;
            _services = services;
            _gameplayStateMachine = gameplayStateMachine;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation)
        {
            _uiFactory.UseContainer(_gameLifetimeScope.Container);
            _gameConsoleService.UseContainer(_gameLifetimeScope.Container);
            
            List<UniTask> initializationTasks = _services.Where(service => service.IsInitialized == false).Select(service => service.Initialize()).ToList();
            await UniTask.WhenAll(initializationTasks);
        
            _gameplayStateMachine.SwitchState<StartGameState>();
        }
    }
}