using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.UI;
using KthulhuWantsMe.Source.UI.PlayerHUD;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services.UI
{
    public interface IUIFactory : IInitializableService
    {
        void EnqueueParent(LifetimeScope parent);
        PlayerHUD CreatePlayerHUD();
        MiscUI CreateMiscUI();
        UpgradeWindow CreateUpgradeWindow();
    }

    public class UIFactory : IUIFactory
    {
        public const string PlayerHUDPath = "PlayerHUD";
        public const string MiscUIPath = "MiscUI";
        public const string UpgradeWindowPath = "UpgradeWindow";
        
        public bool IsInitialized { get; set; }
       
        private PlayerHUD _playerHUDPrefab;
        private MiscUI _miscUIPrefab;
        private UpgradeWindow _upgradeWindowPrefab;

        private Scene _uiScene;

        private IObjectResolver _instantiator;
        
        private readonly IResourceManager _resourceManager;
        private readonly ISceneLoader _sceneLoader;


        public UIFactory(IResourceManager resourceManager, ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _resourceManager = resourceManager;
        }
        
        public async UniTask Initialize()
        {
            await _sceneLoader.LoadScene("GameUI", LoadSceneMode.Additive);
            _uiScene = SceneManager.GetSceneByName("GameUI");
            
            _playerHUDPrefab = await _resourceManager.ProvideAssetAsync<PlayerHUD>(PlayerHUDPath);
            _miscUIPrefab = await _resourceManager.ProvideAssetAsync<MiscUI>(MiscUIPath);
            _upgradeWindowPrefab = await _resourceManager.ProvideAssetAsync<UpgradeWindow>(UpgradeWindowPath);
        }

        public void EnqueueParent(LifetimeScope parent)
        {
            _instantiator = parent.Container;
        }

        public PlayerHUD CreatePlayerHUD()
        {
            PlayerHUD playerHUD = _instantiator.Instantiate(_playerHUDPrefab);
            SceneManager.MoveGameObjectToScene(playerHUD.gameObject, _uiScene);
            playerHUD.Initialize();
            return playerHUD;
        }
        
        public MiscUI CreateMiscUI()
        {
            MiscUI miscUI = _instantiator.Instantiate(_miscUIPrefab);
            SceneManager.MoveGameObjectToScene(miscUI.gameObject, _uiScene);
            return miscUI;
        }

        public UpgradeWindow CreateUpgradeWindow()
        {
            UpgradeWindow upgradeWindow = _instantiator.Instantiate(_upgradeWindowPrefab);
            SceneManager.MoveGameObjectToScene(upgradeWindow.gameObject, _uiScene);
            return upgradeWindow;
        }
    }
}