using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        public float Health;

        public void TakeDamage(float damage)
        {
            ReceiveDamage(damage);
            Debug.Log(Health);
        }

        private void ReceiveDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0)
                Die();
        }

        private void Die()
        {
            Debug.Log("Die");
        }
    }
}