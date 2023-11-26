using System;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace KthulhuWantsMe.Source.UI.MainMenu.Settings
{
    public class SettingsWindow : MonoBehaviour, IInjectable
    {
        [SerializeField] private Button _applyButton;
        
        private SettingsService _settingsService;

        [Inject]
        public void Construct(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        
        private void Start()
        {
            _applyButton.onClick.AddListener(ApplySettings);
        }

        private void OnDestroy()
        {
            _applyButton.onClick.RemoveListener(ApplySettings);
        }

        private void ApplySettings()
        {
            _settingsService.ApplyOverrides();
        }
    }
}
