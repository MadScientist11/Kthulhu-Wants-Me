using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.Audio;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.StateMachine.States
{
    public class ReturnToMenuState : IGameplayState
    {
        private readonly AppLifetimeScope _appLifetimeScope;
        private readonly ISceneService _sceneService;
        private readonly IUIService _uiService;
        private readonly IProgressService _progressService;
        private readonly IBackgroundMusicPlayer _backgroundMusicPlayer;

        public ReturnToMenuState(AppLifetimeScope appLifetimeScope, 
                                 ISceneService sceneService, 
                                 IUIService uiService,
                                 IProgressService progressService, 
                                 IBackgroundMusicPlayer backgroundMusicPlayer)
        {
            _backgroundMusicPlayer = backgroundMusicPlayer;
            _uiService = uiService;
            _sceneService = sceneService;
            _appLifetimeScope = appLifetimeScope;
            _progressService = progressService;
        }
        public async void Enter()
        {
            _backgroundMusicPlayer.StopMusic();
            _uiService.ClearUI();
            _progressService.Reset();
            await _sceneService.UnloadSceneAsync(SceneId.Game);
            await _sceneService.LoadSceneInjected(SceneId.MainMenu, LoadSceneMode.Additive, _appLifetimeScope);
        }

        public void Exit()
        {
        }
    }
}