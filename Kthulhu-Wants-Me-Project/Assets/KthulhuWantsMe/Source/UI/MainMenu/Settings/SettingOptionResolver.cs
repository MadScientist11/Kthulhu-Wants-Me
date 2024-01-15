using System;
using KthulhuWantsMe.Source.Infrastructure;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.UI.MainMenu.Settings
{
    public abstract class SettingOptionResolver<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] private SettingOptionView _settingOptionView;
        
        private T _currentValue;

        public T CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                _settingsService.AddOverride(_settingOptionView.SettingId, CurrentValue);
                _settingOptionView.ValueText.text = _currentValue.ToString();
            }
        }

        private SettingsService _settingsService;

        [Inject]
        public void Construct(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        private void Awake()
        {
            IObjectResolver container = LifetimeScope.Find<LifetimeScope>().Container;
            container.Inject(gameObject);
        }

        private void Start()
        {
            CurrentValue = (T)_settingsService.Get(_settingOptionView.SettingId);
            
            _settingOptionView.PreviousButton.onClick.AddListener(PreviousOption);
            _settingOptionView.NextButton.onClick.AddListener(NextOption);
        }

        private void OnDestroy()
        {
            _settingOptionView.PreviousButton.onClick.RemoveListener(PreviousOption);
            _settingOptionView.NextButton.onClick.RemoveListener(NextOption);
        }

        private void NextOption()
        {
            CurrentValue = _currentValue.Next();
        }

        private void PreviousOption()
        {
            CurrentValue = _currentValue.Previous();
        }
    }
}