using System;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace KthulhuWantsMe.Source.UI
{
    public class PauseWindow : BaseWindow
    {
        public override WindowId Id => WindowId.PauseWindow;
        
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;
        
        private void Start()
        {
            _quitButton.onClick.AddListener(QuitGame);
            _continueButton.onClick.AddListener(ContinueGame);
            _settingsButton.onClick.AddListener(OpenSettings);
        }

        private void OnDestroy()
        {
            _quitButton.onClick.RemoveListener(ContinueGame);
            _quitButton.onClick.RemoveListener(OpenSettings);
            _quitButton.onClick.RemoveListener(QuitGame);
        }
        
        private void OpenSettings()
        {
            _uiService.OpenWindow(WindowId.SettingsWindow);
        }

        private void ContinueGame()
        {
            Hide();
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

    }
}