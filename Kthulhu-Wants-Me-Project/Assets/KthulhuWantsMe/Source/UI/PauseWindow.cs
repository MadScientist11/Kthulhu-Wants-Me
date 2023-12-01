using System;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.UI
{
    public class PauseWindow : BaseWindow
    {
        public override WindowId Id => WindowId.PauseWindow;
        
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;
        
        private ISceneLoader _sceneLoader;
        private IProgressService _progressService;

        [Inject]
        public void Construct(ISceneLoader sceneLoader, IProgressService progressService)
        {
            _progressService = progressService;
            _sceneLoader = sceneLoader;
        }
        
        private void Start()
        {
            _quitButton.onClick.AddListener(ReturnToMenu);
            _continueButton.onClick.AddListener(ContinueGame);
            _settingsButton.onClick.AddListener(OpenSettings);
        }

        private void OnDestroy()
        {
            _quitButton.onClick.RemoveListener(ContinueGame);
            _quitButton.onClick.RemoveListener(OpenSettings);
            _quitButton.onClick.RemoveListener(ReturnToMenu);
        }
        
        private void OpenSettings()
        {
            _uiService.OpenWindow(WindowId.SettingsWindow);
        }

        private void ContinueGame()
        {
            Hide();
        }

        private async void ReturnToMenu()
        {
            await _sceneLoader.UnloadSceneAsync(GameConstants.Scenes.GameSceneName);
            _uiService.ClearUI();
            LifetimeScope lifetimeScope = LifetimeScope.Find<AppLifetimeScope>();
            _progressService.Reset();
            await _sceneLoader.LoadSceneInjected("MainMenu", LoadSceneMode.Additive, lifetimeScope);
        }

    }
}