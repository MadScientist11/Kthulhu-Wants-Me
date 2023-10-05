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
    public interface IUIFactory
    {
        UniTask<PlayerHUD> CreatePlayerHUD();
        UniTask<MiscUI> CreateMiscUI();
        UniTask<UpgradeWindow> CreateUpgradeWindow();
    }

    public class UIFactory : IUIFactory
    {
        public const string PlayerHUDPath = "PlayerHUD";
        public const string MiscUIPath = "MiscUI";
        public const string UpgradeWindowPath = "UpgradeWindow";
        
        public const string GameUIPath = "GameUI";

        
        private bool _gameUISceneLoaded;
        private bool _gameUISceneLoading;

        private readonly IResourceManager _resourceManager;
        private readonly IObjectResolver _instantiator;
        private readonly ISceneLoader _sceneLoader;

        public UIFactory(IResourceManager resourceManager, ISceneLoader sceneLoader, IObjectResolver instantiator)
        {
            _sceneLoader = sceneLoader;
            _instantiator = instantiator;
            _resourceManager = resourceManager;
        }

        public async UniTask<PlayerHUD> CreatePlayerHUD()
        {
            PlayerHUD prefab = await _resourceManager.ProvideAsset<PlayerHUD>(PlayerHUDPath);
            PlayerHUD playerHUD = LifetimeScope.Find<GameUILifetimeScope>().Container.Instantiate(prefab);
            playerHUD.Initialize();
            return playerHUD;
        }

        public async UniTask<MiscUI> CreateMiscUI()
        {
            MiscUI prefab = await _resourceManager.ProvideAsset<MiscUI>(MiscUIPath);
            MiscUI miscUI = LifetimeScope.Find<GameUILifetimeScope>().Container.Instantiate(prefab);
            return miscUI;
        }
        
        public async UniTask<UpgradeWindow> CreateUpgradeWindow()
        {
            UpgradeWindow prefab = await _resourceManager.ProvideAsset<UpgradeWindow>(UpgradeWindowPath);
            UpgradeWindow miscUI = LifetimeScope.Find<GameUILifetimeScope>().Container.Instantiate(prefab);
            return miscUI;
        }


        public bool IsInitialized { get; set; }
    }
}