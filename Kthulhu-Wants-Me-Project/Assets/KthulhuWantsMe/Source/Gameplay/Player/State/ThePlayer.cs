using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.SkillTreeSystem;
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

        public bool IsDead { get; set; }
        public bool IsFullHp => Math.Abs(CurrentHp - MaxHealth) < 0.01f;
        public float CurrentHp => _playerStats.CurrentHp;

        public float MaxHealth => _playerStats.MainStats[StatType.MaxHealth];
        public float BaseDamage => _playerStats.MainStats[StatType.BaseDamage];
        public float MaxStamina => 100;

        private float RegenRate
        {
            get { return (MaxStamina / _playerStats.MainStats[StatType.EvadeCooldown]); }
        }

        public PlayerStats PlayerStats => _playerStats;

        private PlayerStats _playerStats;

        private readonly PlayerConfiguration _playerConfiguration;
        private float _regenAccumulation;

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
            if (Time.timeScale == 0)
            {
                return;
            }
            
            ModifyCurrentStamina(RegenRate * Time.deltaTime);

            if (_playerStats.AcquiredSkills.Contains(SkillId.HealthRegen))
            {
                _regenAccumulation += _playerConfiguration.HealthRegenRate * Time.deltaTime;
                if (_regenAccumulation >= 1f)
                {
                    ModifyCurrentHp(_regenAccumulation);
                    _regenAccumulation = 0;
                }
            }
        }

        public void TakeDamage(IDamageProvider damageProvider)
        {
            if (_playerStats.Immortal)
                return;

            if (ModifyCurrentHp(-damageProvider.ProvideDamage()))
            {
                TookDamage?.Invoke(damageProvider);
                if (damageProvider is IBuffDebuff)
                {
                    return;
                }

                SetPlayerInvincibleAfterDamageFor(_playerConfiguration.InvinciblityAfterAttackTime).Forget();
            }
        }

        public void Heal(float amount)
        {
            ModifyCurrentHp(amount);
        }

        private bool ModifyCurrentHp(float value)
        {
            if (IsDead)
                return false;

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
            IsDead = true;
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