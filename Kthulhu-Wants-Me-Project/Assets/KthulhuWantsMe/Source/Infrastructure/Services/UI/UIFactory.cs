using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using KthulhuWantsMe.Source.UI;
using KthulhuWantsMe.Source.UI.MainMenu.Settings;
using KthulhuWantsMe.Source.UI.PlayerHUD;
using KthulhuWantsMe.Source.UI.PlayerHUD.TooltipSystem;
using QFSW.QC;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services.UI
{
    public interface IUIFactory : IInitializableService
    {
        Scene UIScene { get; }
        void EnqueueParent(LifetimeScope parent);
        PlayerHUD CreatePlayerHUD();
        MiscUI CreateMiscUI();
        UpgradeWindow CreateUpgradeWindow();
        PauseWindow CreatePauseWindow();
        TryAgainWindow CreateDefeatWindow();
        SettingsWindow CreateSettingsWindow();
        Tooltip CreateTooltip();
        QuantumConsole CreateConsoleUI();
    }

    public class UIFactory : IUIFactory
    {
        public const string PlayerHUDPath = "PlayerHUD";
        public const string MiscUIPath = "MiscUI";
        public const string UpgradeWindowPath = "UpgradeWindow";
        public const string PauseWindowPath = "PauseWindow";
        public const string TryAgainWindowPath = "TryAgainWindow";
        public const string SettingsWindowPath = "SettingsWindow";
        public const string TooltipPath = "Tooltip";
        public const string ConsolePath = "InGameConsole";

        public bool IsInitialized { get; set; }
        public Scene UIScene => _uiScene;

        private PlayerHUD _playerHUDPrefab;
        private MiscUI _miscUIPrefab;
        private UpgradeWindow _upgradeWindowPrefab;
        private PauseWindow _pauseWindowPrefab;
        private TryAgainWindow _tryAgainWindowPrefab;
        private SettingsWindow _settingsWindowPrefab;
        private Tooltip _tooltipPrefab;
        private QuantumConsole _consolePrefab;

        private Scene _uiScene;

        private IObjectResolver _instantiator;
        private IObjectResolver _defaultResolver;

        private readonly IResourceManager _resourceManager;
        private readonly ISceneLoader _sceneLoader;


        public UIFactory(IResourceManager resourceManager, ISceneLoader sceneLoader, IObjectResolver instantiator)
        {
            _sceneLoader = sceneLoader;
            _resourceManager = resourceManager;
            _instantiator = instantiator;
            _defaultResolver = instantiator;
        }

        public async UniTask Initialize()
        {
            await _sceneLoader.LoadScene("GameUI", LoadSceneMode.Additive);
            _uiScene = SceneManager.GetSceneByName("GameUI");

            _playerHUDPrefab = await _resourceManager.ProvideAssetAsync<PlayerHUD>(PlayerHUDPath);
            _miscUIPrefab = await _resourceManager.ProvideAssetAsync<MiscUI>(MiscUIPath);
            _upgradeWindowPrefab = await _resourceManager.ProvideAssetAsync<UpgradeWindow>(UpgradeWindowPath);
            _pauseWindowPrefab = await _resourceManager.ProvideAssetAsync<PauseWindow>(PauseWindowPath);
            _tryAgainWindowPrefab = await _resourceManager.ProvideAssetAsync<TryAgainWindow>(TryAgainWindowPath);
            _tooltipPrefab = await _resourceManager.ProvideAssetAsync<Tooltip>(TooltipPath);
            _consolePrefab = await _resourceManager.ProvideAssetAsync<QuantumConsole>(ConsolePath);
            _settingsWindowPrefab = await _resourceManager.ProvideAssetAsync<SettingsWindow>(SettingsWindowPath);
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

        public PauseWindow CreatePauseWindow()
        {
            PauseWindow pauseWindow = _instantiator.Instantiate(_pauseWindowPrefab);
            SceneManager.MoveGameObjectToScene(pauseWindow.gameObject, _uiScene);
            return pauseWindow;
        }

        public TryAgainWindow CreateDefeatWindow()
        {
            TryAgainWindow tryAgainWindow = _instantiator.Instantiate(_tryAgainWindowPrefab);
            SceneManager.MoveGameObjectToScene(tryAgainWindow.gameObject, _uiScene);
            return tryAgainWindow;
        }

        public SettingsWindow CreateSettingsWindow()
        {
            if (_instantiator == null)
            {
                _instantiator = _defaultResolver;
            }

            SettingsWindow settingsWindow;
            try
            {
                settingsWindow  = _instantiator.Instantiate(_settingsWindowPrefab);

            }
            catch (Exception e)
            {
                settingsWindow = _defaultResolver.Instantiate(_settingsWindowPrefab);
            }
            SceneManager.MoveGameObjectToScene(settingsWindow.gameObject, _uiScene);
            return settingsWindow;
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