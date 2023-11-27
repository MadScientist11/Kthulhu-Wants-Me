using System;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace KthulhuWantsMe.Source.UI.MainMenu
{
    public class MainMenuWindow : MonoBehaviour, IInjectable
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _quitButton;

        private IUIService _uiService;
        private ISceneLoader _sceneLoader;
        private IDataProvider _dataProvider;
        private AppLifetimeScope _appLifetimeScope;

        [Inject]
        public void Construct(IUIService uiService, ISceneLoader sceneLoader, IDataProvider dataProvider,
            AppLifetimeScope appLifetimeScope)
        {
            _appLifetimeScope = appLifetimeScope;
            _dataProvider = dataProvider;
            _sceneLoader = sceneLoader;
            _uiService = uiService;
        }

        private void Start()
        {
            _startGameButton.onClick.AddListener(StartGame);
            _optionsButton.onClick.AddListener(OpenSettings);
            _quitButton.onClick.AddListener(Quit);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveListener(StartGame);
            _optionsButton.onClick.RemoveListener(OpenSettings);
            _quitButton.onClick.RemoveListener(Quit);
        }

        private void StartGame()
        {
            _sceneLoader.UnloadSceneAsync("MainMenu");
            _sceneLoader
                .LoadSceneInjected(_dataProvider.GameConfig.MainScene, LoadSceneMode.Additive, _appLifetimeScope)
                .Forget();
        }

        private void OpenSettings()
        {
            _uiService.OpenWindow(WindowId.SettingsWindow);
        }

        private void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}