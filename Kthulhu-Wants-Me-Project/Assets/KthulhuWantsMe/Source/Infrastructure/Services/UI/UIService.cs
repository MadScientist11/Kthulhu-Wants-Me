using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.UI.PlayerHUD;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEngine.SceneManagement;
using VContainer;

namespace KthulhuWantsMe.Source.Infrastructure.Services.UI
{
    public interface IUIService : IInitializableService
    {
        UniTaskVoid ShowPlayerHUD();
        void HidePlayerHUD();
    }

    public class UIService : IUIService
    {
        public PlayerHUD PlayerHUD { get; private set; }

        public bool IsInitialized { get; set; }

        public const string GameUIPath = "GameUI";

        private bool _gameUISceneLoaded;
        private bool _gameUISceneLoading;
        
        private ISceneLoader _sceneLoader;
        
        private IUIFactory _uiFactory;

        [Inject]
        public void Construct(ISceneLoader sceneLoader, IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
        }
        
        public UniTask Initialize()
        {
            IsInitialized = true;
            return UniTask.CompletedTask;
        }

        public void OpenWindow(WindowId windowId)
        {
            
        }

        public void OpenPopUp(PopUpId popUpId)
        {
            
        }
        
        public void ShowNotification() { }

        public async UniTaskVoid ShowPlayerHUD()
        {
            await LoadGameUISceneIfNeeded();

            if (PlayerHUD == null)
            {
                PlayerHUD = await _uiFactory.CreatePlayerHUD();
            } 
            
            PlayerHUD.Show();
        }
        
        public void HidePlayerHUD()
        {
            if (PlayerHUD != null)
            {
                PlayerHUD.Hide();
            }
        }

        private async UniTask LoadGameUISceneIfNeeded()
        {
            if(_gameUISceneLoaded || _gameUISceneLoading)
                return;

            _gameUISceneLoading = true;
            await _sceneLoader.LoadScene(GameUIPath, LoadSceneMode.Additive);
            _gameUISceneLoading = false;
            _gameUISceneLoaded = true;
        }
    }
}