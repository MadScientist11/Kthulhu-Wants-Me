using System;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using TMPro;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.UI
{
    public class MiscUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveCountdownText;

        public void UpdateWaveCountdownText(int countdown)
        {
            _waveCountdownText.text = countdown.ToString();
        }
    }
}