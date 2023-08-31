using System;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        public Action<float> OnHealthChanged;
        public Action OnDied;
        
        private float _currentHealth;
        
        private TentacleConfiguration _tentacleConfiguration;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _tentacleConfiguration = dataProvider.TentacleConfig;
            RestoreHealth();
        }

        public void TakeDamage(float damage)
        {
            ReceiveDamage(damage);
            Debug.Log($"Enemy was hit {damage}");
        }

        private void ReceiveDamage(float damage)
        {
            _currentHealth -= damage;
            OnHealthChanged?.Invoke(_currentHealth);
            
            if (_currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            OnDied?.Invoke();
            Debug.Log("Die");
        }

        public void RestoreHealth()
        {
            _currentHealth = _tentacleConfiguration.MaxHealth;
        }
    }
}