using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace KthulhuWantsMe.Source.UI.MainMenu.Settings
{
    public class SettingSliderView : MonoBehaviour
    {
        public Slider Slider;
        public SettingId SettingId;

        private int _currentValue;
        
        private SettingsService _settingsService;

        [Inject]
        public void Construct(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        private void Start()
        {
            Slider.minValue = 0;
            Slider.maxValue = 100;

            if (SettingId != SettingId.None)
            {
                _currentValue = (int)(SettingGradation)_settingsService.Get(SettingId);
                Slider.value = _currentValue;
            }

            Slider.onValueChanged.AddListener(SnapToTens);
        }

        private void OnDestroy()
        {
            Slider.onValueChanged.RemoveListener(SnapToTens);
        }

        private void SnapToTens(float value)
        {
            int snappedValue = Mathf.RoundToInt(value / 10.0f) * 10;
            Slider.value = snappedValue;
            _settingsService.AddOverride(SettingId, (SettingGradation)snappedValue);
        }
    }
}