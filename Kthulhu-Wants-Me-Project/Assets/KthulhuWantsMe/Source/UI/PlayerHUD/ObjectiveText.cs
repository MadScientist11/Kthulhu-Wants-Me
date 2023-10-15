using System;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using TMPro;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.UI.PlayerHUD
{
    public class ObjectiveText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _objectiveText;
        
        private IWaveSystemDirector _waveSystemDirector;

        [Inject]
        public void Construct(IWaveSystemDirector waveSystemDirector)
        {
            _waveSystemDirector = waveSystemDirector;
        }
        
        private void OnEnable()
        {
            ShowObjectiveText();

        }

        private void ShowObjectiveText()
        {
            _objectiveText.text = _waveSystemDirector.CurrentWaveState.WaveObjective switch
            {
                WaveObjective.KillAllEnemies => "Defeat All Enemies",
                WaveObjective.KillTentaclesSpecial => "Defeat All Tentacles",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
