using System;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using TMPro;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.UI.PlayerHUD
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveLossTimerText;
        
        public void UpdateTImerText(int countdown)
        {
            TimeSpan span = TimeSpan.FromSeconds(countdown);
            _waveLossTimerText.text = string.Format("{0}:{1:00}", 
                (int)span.TotalMinutes, 
                span.Seconds);
        }
    }
}
