using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.Infrastructure.Services.UI.Window;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace KthulhuWantsMe.Source.UI.MainMenu.Settings
{
    public class SettingsWindow : BaseWindow
    {
        public override WindowId Id => WindowId.SettingsWindow;
        
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _backButton;
        
        [SerializeField] private Button _audioButton;
        [SerializeField] private Button _displayButton;
        
        [SerializeField] private Transform _audioSettings;
        [SerializeField] private Transform _displaySettings;

        private readonly List<Transform> _allTabs = new();
        private SettingsService _settingsService;
        private IUIService _uiService;

        [Inject]
        public void Construct(SettingsService settingsService, IUIService uiService)
        {
            _uiService = uiService;
            _settingsService = settingsService;
        }
        
        private void Start()
        {
            _allTabs.Add(_audioSettings);
            _allTabs.Add(_displaySettings);
            
            _audioButton.onClick.AddListener(OpenAudioSettings);
            _displayButton.onClick.AddListener(OpenDisplaySettings);
            
            _applyButton.onClick.AddListener(ApplySettings);
            _backButton.onClick.AddListener(CloseWindow);
        }

        private void OnDestroy()
        {
            _audioButton.onClick.RemoveListener(OpenAudioSettings);
            _displayButton.onClick.RemoveListener(OpenDisplaySettings);
            
            _applyButton.onClick.RemoveListener(ApplySettings);
            _backButton.onClick.RemoveListener(CloseWindow);
        }

        private void OpenAudioSettings() 
            => SwitchTab(0);
        
        private void OpenDisplaySettings()
            => SwitchTab(1);

        private void SwitchTab(int index)
        {
            foreach (Transform tab in _allTabs)
            {
                tab.gameObject.SwitchOff();
            }
            
            _allTabs[index].gameObject.SwitchOn();
        }
        
        
        private void CloseWindow()
        {
            _uiService.CloseWindow(WindowId.SettingsWindow);
        }
        
        private void ApplySettings()
        {
            _settingsService.ApplyOverrides();
        }

    }
}
