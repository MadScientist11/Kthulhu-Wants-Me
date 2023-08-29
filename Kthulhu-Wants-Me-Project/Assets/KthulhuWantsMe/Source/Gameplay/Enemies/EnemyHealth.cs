using System;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        public Action<float> OnHealthChanged;
        public float Health;

        public void TakeDamage(float damage)
        {
            ReceiveDamage(damage);
            Debug.Log($"Enemy was hit {damage}");
        }

        private void ReceiveDamage(float damage)
        {
            Health -= damage;
            OnHealthChanged?.Invoke(Health);
            
            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            Debug.Log("Die");
        }
    }
}