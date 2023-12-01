using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class RestartGameState : IGameplayState
    {
        private readonly AppLifetimeScope _appLifetimeScope;
        private readonly ISceneLoader _sceneLoader;
        private readonly IDataProvider _dataProvider;
        private readonly IUIService _uiService;
        private IProgressService _progressService;

        public RestartGameState(AppLifetimeScope appLifetimeScope, ISceneLoader sceneLoader, IDataProvider dataProvider, IUIService uiService,
            IProgressService progressService)
        {
            _progressService = progressService;
            _uiService = uiService;
            _dataProvider = dataProvider;
            _sceneLoader = sceneLoader;
            _appLifetimeScope = appLifetimeScope;
        }
        
        public async void Enter()
        {
            _uiService.ClearUI();
            await _sceneLoader.UnloadSceneAsync(GameConstants.Scenes.GameSceneName);
            LifetimeScope lifetimeScope = LifetimeScope.Find<AppLifetimeScope>();
            _progressService.Reset();
            await _sceneLoader.LoadSceneInjected(GameConstants.Scenes.GameSceneName, LoadSceneMode.Additive, lifetimeScope);
            
        }

        public void Exit()
        {
        }
    }
}