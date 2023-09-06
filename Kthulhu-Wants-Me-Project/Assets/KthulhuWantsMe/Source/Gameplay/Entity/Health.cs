﻿using System;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public interface IHealable
    {
        void Heal(float amount);
    }

    public abstract class Health : MonoBehaviour, IDamageable, IHealable
    {
        public abstract float MaxHealth { get; }

        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                float newHealth = Mathf.Clamp(value, 0, MaxHealth);
                
                
                if (newHealth < _currentHealth)
                    TookDamage?.Invoke();

                _currentHealth = newHealth;

                Changed?.Invoke(_currentHealth);

                if (_currentHealth == 0)
                    Died?.Invoke();
            }
        }

        public event Action<float> Changed;
        public event Action TookDamage;
        public event Action Died;

        private float _currentHealth;


        public virtual void TakeDamage(float damage) =>
            CurrentHealth -= damage;

        public virtual void Heal(float amount) =>
            CurrentHealth += amount;


        public void RestoreHp() =>
            CurrentHealth = MaxHealth;
    }
}