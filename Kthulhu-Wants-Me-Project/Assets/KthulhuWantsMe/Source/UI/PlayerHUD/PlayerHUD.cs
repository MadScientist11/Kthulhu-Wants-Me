using System;
using KthulhuWantsMe.Source.Gameplay;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.UI.PlayerHUD
{
    public class PlayerHUD : MonoBehaviour, IUIElement
    {
        [FormerlySerializedAs("SpecialObjectWaveLossTimerUI")] public SpecialObjectiveWaveLossTimerUI specialObjectiveWaveLossTimerUI;
        
        [SerializeField] private IndicatorsUI _indicatorsUI;
        [SerializeField] private HpBar _playerHpBar;
        [SerializeField] private HpBar _playerStaminaBar;

        [SerializeField] private GameObject _objective;
        [SerializeField] private GameObject _waveLossTimer;
        
        private ThePlayer _player;
        private IWaveSystemDirector _waveSystemDirector;

        [Inject]
        public void Construct(ThePlayer player, IWaveSystemDirector waveSystemDirector)
        {
            _waveSystemDirector = waveSystemDirector;
            _player = player;
        }
        
        public void Initialize()
        {
            _playerHpBar.SetNewMax(_player.PlayerStats.MainStats[StatType.MaxHealth]);
            _playerStaminaBar.SetNewMax(_player.PlayerStats.MainStats[StatType.MaxStamina]);
            
            _player.HealthChanged += UpdateHealthBar;

            _waveSystemDirector.WaveStarted += OnWaveStarted;
            _waveSystemDirector.WaveCompleted += OnWaveCompleted;
        }

        private void OnDestroy()
        {
            _player.HealthChanged -= UpdateHealthBar;
        }

        private void Update()
        {
            _playerStaminaBar.SetValue(_player.PlayerStats.CurrentStamina, _player.PlayerStats.MainStats[StatType.MaxStamina]);
        }

        public void Show()
        {
            _indicatorsUI.Enable();
            _playerHpBar.SetValue(_player.PlayerStats.CurrentHp, _player.PlayerStats.MainStats[StatType.MaxHealth]);
        }

        public void Hide()
        {
        }

        private void OnWaveStarted()
        {
            _objective.SwitchOn();
            
            if(_waveSystemDirector.CurrentWaveState.WaveObjective == WaveObjective.KillTentaclesSpecial)
                _waveLossTimer.SwitchOn();
        }

        private void OnWaveCompleted()
        {
            _objective.SwitchOff();
            
            if(_waveSystemDirector.CurrentWaveState.WaveObjective == WaveObjective.KillTentaclesSpecial)
                _waveLossTimer.SwitchOff();
        }

        private void UpdateHealthBar(HealthChange healthChange)
        {
            _playerHpBar.SetValue(healthChange.Current, _player.PlayerStats.MainStats[StatType.MaxHealth]);
        }
        
        private void GrowHealthBar(StatType statType, float newValue)
        {
            if (statType == StatType.MaxHealth)
            {
                _playerHpBar.SetNewMax(_player.PlayerStats.MainStats[StatType.MaxHealth]);
                _playerHpBar.SetValue(_player.PlayerStats.CurrentHp, _player.PlayerStats.MainStats[StatType.MaxHealth]);
            }
        }

        public void HideObjective()
        {
            _objective.SetActive(false);
        }
    }
}
