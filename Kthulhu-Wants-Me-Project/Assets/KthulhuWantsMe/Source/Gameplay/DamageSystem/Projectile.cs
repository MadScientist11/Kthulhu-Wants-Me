using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public class Projectile : WeaponBase
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Collided with {other.name}");
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damageProviders.Sum(provider => provider.ProvideDamage()));
                return;
            }
        }
    }
}