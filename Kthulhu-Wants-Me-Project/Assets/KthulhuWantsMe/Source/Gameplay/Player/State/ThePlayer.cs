﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure;
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

    public class ThePlayer : IInitializable, ITickable
    {
        public event Action<HealthChange> HealthChanged;
        public event Action<IDamageProvider> TookDamage;
        public event Action Died;


        public PlayerInventory Inventory { get; private set; }

        public float CurrentHp => _playerStats.CurrentHp;

        public float MaxHealth => _playerStats.MainStats[StatType.MaxHealth];
        public float BaseDamage => _playerStats.MainStats[StatType.BaseDamage];
        public float MaxStamina => _playerStats.MainStats[StatType.MaxStamina];
        
        public PlayerStats PlayerStats => _playerStats;

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
            RestoreStamina();
        }
        
        public void Tick()
        {
            ModifyCurrentStamina(_playerConfiguration.StaminaRegenRate * Time.deltaTime);
        }

        public void TakeDamage(IDamageProvider damageProvider)
        {
            if (_playerStats.Immortal)
                return;

            if (ModifyCurrentHp(-damageProvider.ProvideDamage()))
            {
                TookDamage?.Invoke(damageProvider);
                SetPlayerInvincibleAfterDamageFor(_playerConfiguration.InvinciblityAfterAttackTime).Forget();
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

        public void ModifyCurrentStamina(float value)
        {
            float previousValue = _playerStats.CurrentStamina;
            _playerStats.CurrentStamina += value;
            _playerStats.CurrentStamina = Mathf.Clamp(_playerStats.CurrentStamina, 0, MaxStamina);
        }

        public void RestoreHp()
        {
            _playerStats.CurrentHp = MaxHealth;
        }
        public void RestoreStamina()
        {
            _playerStats.CurrentStamina = MaxStamina;
        }

        private void Kill()
        {
            Died?.Invoke();
        }

        private async UniTaskVoid SetPlayerInvincibleAfterDamageFor(float seconds)
        {
            _playerStats.Immortal = true;
            await UniTask.Delay((int)(seconds * 1000));
            _playerStats.Immortal = false;
        }
    }
}