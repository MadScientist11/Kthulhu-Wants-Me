﻿using System;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public abstract class Health : MonoBehaviour, IDamageable
    {
        public abstract float MaxHealth { get; }

        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                Changed?.Invoke(_currentHealth);
                TookDamage?.Invoke();

                if (_currentHealth == 0)
                {
                    Died?.Invoke();
                }
            }
        }

        public event Action<float> Changed;
        public event Action TookDamage;
        public event Action Died;

        private float _currentHealth;


        public virtual void TakeDamage(float damage) =>
            CurrentHealth -= damage;

        public void RestoreHp() => 
            CurrentHealth = MaxHealth;
    }

}