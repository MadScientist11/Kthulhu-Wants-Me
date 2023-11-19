using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEngine.SceneManagement;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class RestartGameState : IGameplayState
    {
        private readonly AppLifetimeScope _appLifetimeScope;
        private readonly ISceneLoader _sceneLoader;
        private readonly IDataProvider _dataProvider;
        private readonly IUIService _uiService;


        public RestartGameState(AppLifetimeScope appLifetimeScope, ISceneLoader sceneLoader, IDataProvider dataProvider, IUIService uiService)
        {
            _uiService = uiService;
            _dataProvider = dataProvider;
            _sceneLoader = sceneLoader;
            _appLifetimeScope = appLifetimeScope;
        }
        
        public async void Enter()
        {
            await _sceneLoader.LoadScene("StartUp", LoadSceneMode.Single);
        }

        public void Exit()
        {
        }
    }
}