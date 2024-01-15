using KthulhuWantsMe.Source.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KthulhuWantsMe.Source.UI.MainMenu.Settings
{
    public class SettingOptionView : MonoBehaviour, IInjectable
    {
        public Button PreviousButton;
        public Button NextButton;
        public TextMeshProUGUI ValueText;
        public SettingId SettingId;
    }
}