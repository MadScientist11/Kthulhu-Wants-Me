using System;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.UI;
using KthulhuWantsMe.Source.UI.PlayerHUD;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using Object = UnityEngine.Object;

namespace KthulhuWantsMe.Source.Infrastructure.Services.UI
{
    public interface IUIService : IInitializableService
    {
        MiscUI MiscUIContainer { get; }
        UniTaskVoid ShowPlayerHUD();
        void HidePlayerHUD();
        UniTask InitializeGameUI();
        UniTask<BaseWindow> OpenWindow(WindowId windowId);
        void OpenPopUp(PopUpId popUpId);
    }

    public class UIService : IUIService
    {
        public PlayerHUD PlayerHUD { get; private set; }
        public MiscUI MiscUIContainer { get; private set; }

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
        
        public  UniTask Initialize()
        {
            IsInitialized = true;
            return UniTask.CompletedTask;
        }
        
        public  async UniTask InitializeGameUI()
        {
            await LoadGameUISceneIfNeeded();
            await LoadMiscUIContainer();
        }

        public async UniTask<BaseWindow> OpenWindow(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.UpgradeWindow:
                   return await _uiFactory.CreateUpgradeWindow();
                    break;
            }

            return null;
        }

        public void OpenPopUp(PopUpId popUpId)
        {
            
        }
        
        public void ShowNotification() { }

        public async UniTask<MiscUI> LoadMiscUIContainer()
        {

            if (MiscUIContainer == null)
            {
                MiscUIContainer = await _uiFactory.CreateMiscUI();
            }

            return MiscUIContainer;
        }

        public async UniTaskVoid ShowPlayerHUD()
        {

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
        public async UniTask LoadGameUISceneIfNeeded()
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