using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.Audio;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class ReturnToMenuState : IGameplayState
    {
        private readonly AppLifetimeScope _appLifetimeScope;
        private readonly ISceneService _sceneService;
        private readonly IDataProvider _dataProvider;
        private readonly IUIService _uiService;
        private readonly IProgressService _progressService;
        private IBackgroundMusicPlayer _backgroundMusicPlayer;

        public ReturnToMenuState(AppLifetimeScope appLifetimeScope, ISceneService sceneService, IDataProvider dataProvider, IUIService uiService,
            IProgressService progressService, IBackgroundMusicPlayer backgroundMusicPlayer)
        {
            _backgroundMusicPlayer = backgroundMusicPlayer;
            _uiService = uiService;
            _dataProvider = dataProvider;
            _sceneService = sceneService;
            _appLifetimeScope = appLifetimeScope;
            _progressService = progressService;
        }
        public async void Enter()
        {
            await _sceneService.UnloadSceneAsync(GameConstants.Scenes.GameSceneName);
            _backgroundMusicPlayer.StopMusic();
            _uiService.ClearUI();
            LifetimeScope lifetimeScope = LifetimeScope.Find<AppLifetimeScope>();
            _progressService.Reset();
            await _sceneService.LoadSceneInjected("MainMenu", LoadSceneMode.Additive, lifetimeScope);
        }

        public void Exit()
        {
        }
    }
}