using System;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.UI.PlayerHUD
{
    public class PlayerHUD : MonoBehaviour, IUIElement
    {
        [SerializeField] private IndicatorsUI _indicatorsUI;
        [SerializeField] private HpBar _playerHpBar;
        
        private ThePlayer _player;

        [Inject]
        public void Construct(ThePlayer player)
        {
            _player = player;
        }
        
        public void Initialize()
        {
            _playerHpBar.SetNewMax(_player.PlayerStats.MainStats[StatType.MaxHealth]);
            
            _player.HealthChanged += UpdateHealthBar;
            _player.PlayerStats.StatChanged += GrowHealthBar;
        }

        private void OnDestroy()
        {
            _player.HealthChanged -= UpdateHealthBar;
            _player.PlayerStats.StatChanged -= GrowHealthBar;
        }

        public void Show()
        {
            _indicatorsUI.Enable();
        }

        public void Hide()
        {
            
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
        
    }
}
