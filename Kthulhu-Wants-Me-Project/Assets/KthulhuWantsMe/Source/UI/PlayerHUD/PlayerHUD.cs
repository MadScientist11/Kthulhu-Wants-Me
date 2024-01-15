using KthulhuWantsMe.Source.Gameplay;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.Stats;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using TMPro;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.UI.PlayerHUD
{
    public class PlayerHUD : MonoBehaviour, IUIElement
    {
        public TimerUI _timerUI;
        
        [SerializeField] private IndicatorsUI _indicatorsUI;
        [SerializeField] private HpBar _playerHpBar;
        [SerializeField] private HpBar _playerStaminaBar;

        [SerializeField] private GameObject _objective;
        [SerializeField] private GameObject _waveLossTimer;
        [SerializeField] private GameObject _waveCounter;
        [SerializeField] private TextMeshProUGUI _waveCounterText;
        
        private ThePlayer _player;
        private IWaveSystemDirector _waveSystemDirector;
        private IProgressService _progressService;

        [Inject]
        public void Construct(ThePlayer player, IWaveSystemDirector waveSystemDirector, IProgressService progressService)
        {
            _progressService = progressService;
            _waveSystemDirector = waveSystemDirector;
            _player = player;
        }
        
        public void Initialize()
        {
            _playerHpBar.SetNewMax(_player.PlayerStats.MainStats[StatType.MaxHealth]);
            _playerStaminaBar.SetNewMax(_player.MaxStamina);
            
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
            _playerStaminaBar.SetValue(_player.PlayerStats.CurrentStamina, _player.MaxStamina);

        }

        public void Init()
        {
            _indicatorsUI.Enable();
            _playerHpBar.SetValue(_player.PlayerStats.CurrentHp, _player.PlayerStats.MainStats[StatType.MaxHealth]);
        }

        public void Hide()
        {
            gameObject.SwitchOff();
        }

        private void OnWaveStarted()
        {
            _objective.SwitchOn();
            _waveCounter.SwitchOn();

            _waveCounterText.text = $"{_progressService.ProgressData.CompletedWaveIndex + 2}/10";
            
        
        }

        private void OnWaveCompleted()
        {
            _objective.SwitchOff();
            _waveCounter.SwitchOff();

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
