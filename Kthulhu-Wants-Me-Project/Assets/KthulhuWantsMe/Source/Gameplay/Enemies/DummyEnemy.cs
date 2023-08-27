using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class DummyEnemy : MonoBehaviour, IDamageable
    {
        public float Health;
        public void TakeDamage(float damage)
        {
            Health -= damage;
            Debug.Log(Health);
        }
    }
}