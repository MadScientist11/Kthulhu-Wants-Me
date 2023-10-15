using System;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using TMPro;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.UI.PlayerHUD
{
    public class SpecialObjectiveWaveLossTimerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveLossTimerText;
        
        private IWaveSystemDirector _waveSystemDirector;

        [Inject]
        public void Construct(IWaveSystemDirector waveSystemDirector)
        {
            _waveSystemDirector = waveSystemDirector;
        }

        private void OnEnable()
        {
            if(_waveSystemDirector.CurrentWaveScenario is not KillTentaclesSpecialScenario specialScenario)
                return;

            specialScenario.WaveLossTimerTick += OnWaveLossTimerTick;
        }

        private void OnDisable()
        {
            if(_waveSystemDirector.CurrentWaveScenario is not KillTentaclesSpecialScenario specialScenario)
                return;
            
            specialScenario.WaveLossTimerTick -= OnWaveLossTimerTick;
        }

        private void OnWaveLossTimerTick(int seconds)
        {
            UpdateWaveCountdownText(seconds);
        }

        private void UpdateWaveCountdownText(int countdown)
        {
            TimeSpan span = TimeSpan.FromSeconds(countdown);
            _waveLossTimerText.text = string.Format("{0}:{1:00}", 
                (int)span.TotalMinutes, 
                span.Seconds);
        }
    }
}
