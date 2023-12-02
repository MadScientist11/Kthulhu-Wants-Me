using System;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
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

        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        public void Construct(GameplayStateMachine gameplayStateMachine)
        {
            _gameplayStateMachine = gameplayStateMachine;
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

        private void ReturnToMenu()
        {
            _gameplayStateMachine.SwitchState<ReturnToMenuState>();
        }
    }
}