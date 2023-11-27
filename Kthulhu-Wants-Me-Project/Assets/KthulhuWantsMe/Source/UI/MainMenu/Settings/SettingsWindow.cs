using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace KthulhuWantsMe.Source.UI.MainMenu.Settings
{
    public class SettingsWindow : MonoBehaviour, IInjectable
    {
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _backButton;
        
        [SerializeField] private Button _audioButton;
        [SerializeField] private Button _displayButton;
        [SerializeField] private Button _controlsButton;
        
        [SerializeField] private Transform _audioSettings;
        [SerializeField] private Transform _displaySettings;
        [SerializeField] private Transform _constrolsSettings;

        private SettingsService _settingsService;
        private readonly List<Transform> _allTabs = new();

        [Inject]
        public void Construct(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        
        private void Start()
        {
            _allTabs.Add(_audioSettings);
            _allTabs.Add(_displaySettings);
            _allTabs.Add(_constrolsSettings);
            
            _audioButton.onClick.AddListener(OpenAudioSettings);
            _displayButton.onClick.AddListener(OpenDisplaySettings);
            _controlsButton.onClick.AddListener(OpenControlsSettings);
            
            _applyButton.onClick.AddListener(ApplySettings);
            _backButton.onClick.AddListener(CloseWindow);
        }

        private void OnDestroy()
        {
            _audioButton.onClick.RemoveListener(OpenAudioSettings);
            _displayButton.onClick.RemoveListener(OpenDisplaySettings);
            _controlsButton.onClick.RemoveListener(OpenControlsSettings);
            
            _applyButton.onClick.RemoveListener(ApplySettings);
            _backButton.onClick.RemoveListener(CloseWindow);
        }

        private void OpenAudioSettings() 
            => SwitchTab(0);
        
        private void OpenDisplaySettings()
            => SwitchTab(1);

        private void OpenControlsSettings() 
            => SwitchTab(2);

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
            Destroy(gameObject);
        }
        
        private void ApplySettings()
        {
            _settingsService.ApplyOverrides();
        }
    }
}
