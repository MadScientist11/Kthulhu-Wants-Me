using System;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.UI.Compass;
using TMPro;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.UI
{
    public class MiscUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveCountdownText;
        [SerializeField] private CompassUI _compassUI;
        public CompassUI GetCompassUI()
        {
            return _compassUI;
        }
        public void UpdateWaveCountdownText(int countdown)
        {
            _waveCountdownText.text = countdown.ToString();
        }
    }
}