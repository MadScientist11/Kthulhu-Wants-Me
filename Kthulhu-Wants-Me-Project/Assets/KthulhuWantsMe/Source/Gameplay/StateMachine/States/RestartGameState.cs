using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEngine.SceneManagement;

namespace KthulhuWantsMe.Source.Gameplay.StateMachine.States
{
    public class RestartGameState : IGameplayState
    {
        private readonly AppLifetimeScope _appLifetimeScope;
        private readonly ISceneService _sceneService;
        private readonly IUIService _uiService;
        private readonly IProgressService _progressService;

        public RestartGameState(AppLifetimeScope appLifetimeScope, 
                                ISceneService sceneService,
                                IUIService uiService,
                                IProgressService progressService)
        {
            _progressService = progressService;
            _uiService = uiService;
            _sceneService = sceneService;
            _appLifetimeScope = appLifetimeScope;
        }
        
        public async void Enter()
        {
            _uiService.ClearUI();
            _progressService.Reset();
            await _sceneService.UnloadSceneAsync(SceneId.Game);
            await _sceneService.LoadSceneInjected(SceneId.Game, LoadSceneMode.Additive, _appLifetimeScope);
        }

        public void Exit()
        {
        }
    }
}