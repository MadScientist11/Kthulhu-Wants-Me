using System;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.Player.State
{
    public class HealthChange
    {
        public float Previous;

        public float Current;

        public HealthChange(float prev, float current)
        {
            Previous = prev;
            Current = current;
        }
    }

    public class ThePlayer : IInitializable
    {
        public event Action<HealthChange> HealthChanged;
        public event Action<IDamageProvider> TookDamage;
        public event Action Died;
        
        
        public PlayerInventory Inventory { get; private set; }

        public float CurrentHp => _playerStats.CurrentHp;

        public float MaxHealth => _playerStats.BaseStats[StatType.MaxHealth];
        public float BaseDamage => _playerStats.BaseStats[StatType.BaseDamage];


        private PlayerStats _playerStats;

        private readonly PlayerConfiguration _playerConfiguration;

        public ThePlayer(IDataProvider dataProvider)
        {
            _playerConfiguration = dataProvider.PlayerConfig;
            Inventory = new PlayerInventory();
        }

        public void Initialize()
        {
            _playerStats = new PlayerStats(_playerConfiguration);
            RestoreHp();
        }
        
        public void TakeDamage(IDamageProvider damageProvider)
        {
            if (ModifyCurrentHp(-damageProvider.ProvideDamage()))
            {
                TookDamage?.Invoke(damageProvider);
            }
        }

        public void Heal(float amount)
        {
            ModifyCurrentHp(amount);
        }

        private bool ModifyCurrentHp(float value)
        {
            float previousValue = _playerStats.CurrentHp;
            _playerStats.CurrentHp += value;
            _playerStats.CurrentHp = Mathf.Clamp(_playerStats.CurrentHp, 0, MaxHealth);

            HealthChanged?.Invoke(new HealthChange(previousValue, _playerStats.CurrentHp));

            if (_playerStats.CurrentHp <= 0f)
                Kill();

            return true;
        }

        public void RestoreHp()
        {
            _playerStats.CurrentHp = MaxHealth;
        }

        private void Kill()
        {
            Died?.Invoke();
        }
    }
}