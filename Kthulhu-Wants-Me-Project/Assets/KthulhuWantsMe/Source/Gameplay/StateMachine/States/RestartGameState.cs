using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.StateMachine.States
{
    public class RestartGameState : IGameplayState
    {
        private readonly AppLifetimeScope _appLifetimeScope;
        private readonly ISceneService _sceneService;
        private readonly IDataProvider _dataProvider;
        private readonly IUIService _uiService;
        private IProgressService _progressService;

        public RestartGameState(AppLifetimeScope appLifetimeScope, ISceneService sceneService, IDataProvider dataProvider, IUIService uiService,
            IProgressService progressService)
        {
            _progressService = progressService;
            _uiService = uiService;
            _dataProvider = dataProvider;
            _sceneService = sceneService;
            _appLifetimeScope = appLifetimeScope;
        }
        
        public async void Enter()
        {
            _uiService.ClearUI();
            await _sceneService.UnloadSceneAsync(GameConstants.Scenes.GameSceneName);
            LifetimeScope lifetimeScope = LifetimeScope.Find<AppLifetimeScope>();
            _progressService.Reset();
            await _sceneService.LoadSceneInjected(GameConstants.Scenes.GameSceneName, LoadSceneMode.Additive, lifetimeScope);
            
        }

        public void Exit()
        {
        }
    }
}