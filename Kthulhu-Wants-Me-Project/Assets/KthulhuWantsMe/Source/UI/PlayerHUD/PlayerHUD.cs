using System;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.UI.PlayerHUD
{
    public class PlayerHUD : MonoBehaviour, IUIElement
    {
        [SerializeField] private HpBar _playerHpBar;
        
        private ThePlayer _player;

        [Inject]
        public void Construct(ThePlayer player)
        {
            _player = player;
        }
        
        public void Initialize()
        {
            _player.HealthChanged += UpdateHealthBar;
        }

        private void OnDestroy()
        {
            _player.HealthChanged -= UpdateHealthBar;
        }

        public void Show()
        {
            
        }

        public void Hide()
        {
            
        }

        private void UpdateHealthBar(HealthChange healthChange)
        {
            _playerHpBar.SetValue(healthChange.Current, _player.MaxHealth);
        }
        
    }
}
