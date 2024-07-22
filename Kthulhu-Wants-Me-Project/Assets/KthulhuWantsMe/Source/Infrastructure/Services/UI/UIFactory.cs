using System;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using KthulhuWantsMe.Source.UI;
using KthulhuWantsMe.Source.UI.MainMenu.Settings;
using KthulhuWantsMe.Source.UI.PlayerHUD;
using KthulhuWantsMe.Source.UI.PlayerHUD.TooltipSystem;
using QFSW.QC;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services.UI
{
    public interface IUIFactory : IInitializableService
    {
        BaseWindow Create(WindowId windowId);
        void UseContainer(IObjectResolver container);
        PlayerHUD CreatePlayerHUD();
        Tooltip CreateTooltip();
        QuantumConsole CreateConsoleUI();
    }

    public class UIFactory : IUIFactory
    {
        public const string PlayerHUDPath = "PlayerHUD";
        public const string UpgradeWindowPath = "UpgradeWindow";
        public const string PauseWindowPath = "PauseWindow";
        public const string TryAgainWindowPath = "TryAgainWindow";
        public const string SettingsWindowPath = "SettingsWindow";
        public const string TooltipPath = "Tooltip";
        public const string ConsolePath = "InGameConsole";

        public bool IsInitialized { get; set; }
        public Scene UIScene => _uiScene;

        private PlayerHUD _playerHUDPrefab;
        private UpgradeWindow _upgradeWindowPrefab;
        private PauseWindow _pauseWindowPrefab;
        private TryAgainWindow _tryAgainWindowPrefab;
        private SettingsWindow _settingsWindowPrefab;
        private Tooltip _tooltipPrefab;
        private QuantumConsole _consolePrefab;

        private Scene _uiScene;

        private IObjectResolver _instantiator;

        private readonly IResourceManager _resourceManager;
        private readonly ISceneService _sceneService;


        public UIFactory(IResourceManager resourceManager, ISceneService sceneService, IObjectResolver instantiator)
        {
            _sceneService = sceneService;
            _resourceManager = resourceManager;
            _instantiator = instantiator;
        }

        public async UniTask Initialize()
        {
            await _sceneService.LoadScene(SceneId.UI, LoadSceneMode.Additive);
            _uiScene = SceneManager.GetSceneByBuildIndex((int)SceneId.UI);

            _playerHUDPrefab = await _resourceManager.ProvideAssetAsync<PlayerHUD>(PlayerHUDPath);
            _upgradeWindowPrefab = await _resourceManager.ProvideAssetAsync<UpgradeWindow>(UpgradeWindowPath);
            _pauseWindowPrefab = await _resourceManager.ProvideAssetAsync<PauseWindow>(PauseWindowPath);
            _tryAgainWindowPrefab = await _resourceManager.ProvideAssetAsync<TryAgainWindow>(TryAgainWindowPath);
            _tooltipPrefab = await _resourceManager.ProvideAssetAsync<Tooltip>(TooltipPath);
            _consolePrefab = await _resourceManager.ProvideAssetAsync<QuantumConsole>(ConsolePath);
            _settingsWindowPrefab = await _resourceManager.ProvideAssetAsync<SettingsWindow>(SettingsWindowPath);
        }

        public void UseContainer(IObjectResolver container)
        {
            _instantiator = container;
        }

        public PlayerHUD CreatePlayerHUD()
        {
            PlayerHUD playerHUD = _instantiator.Instantiate(_playerHUDPrefab);
            SceneManager.MoveGameObjectToScene(playerHUD.gameObject, _uiScene);
            playerHUD.Initialize();
            return playerHUD;
        }
        
        public BaseWindow Create(WindowId windowId)
        {
            BaseWindow window = windowId switch
            {
                WindowId.UpgradeWindow =>    _instantiator.Instantiate(_upgradeWindowPrefab), 
                WindowId.PauseWindow => _instantiator.Instantiate(_pauseWindowPrefab),
                WindowId.DefeatWindow => _instantiator.Instantiate(_tryAgainWindowPrefab),
                WindowId.SettingsWindow => _instantiator.Instantiate(_settingsWindowPrefab),
                _ => throw new Exception("Unidentified window type!")
            };
            SceneManager.MoveGameObjectToScene(window.gameObject, _uiScene);
            return window;
        }

        public Tooltip CreateTooltip()
        {
            Tooltip tooltip = _instantiator.Instantiate(_tooltipPrefab);
            SceneManager.MoveGameObjectToScene(tooltip.gameObject, _uiScene);
            return tooltip;
        }

        public QuantumConsole CreateConsoleUI()
        {
            QuantumConsole console = _instantiator.Instantiate(_consolePrefab);
            SceneManager.MoveGameObjectToScene(console.gameObject, _uiScene);
            return console;
        }
    }
}