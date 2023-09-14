using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.DamageSystem
{
    public interface IDamageable
    {
        Transform Transform { get; }
        void TakeDamage(float damage);
        void TakeDamage(float damage, IDamageProvider damageProvider)
            => TakeDamage(damage);
    }
}