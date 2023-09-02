using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Entity
{
    public abstract class Attack : MonoBehaviour, IDamageProvider
    {
        protected abstract float BaseDamage { get; }

        public virtual float ProvideDamage() => 
            BaseDamage;

        protected virtual void OnAttack() { }

        protected virtual void OnAttackEnd() { }

        protected void ApplyDamage(IDamageable damageable) => 
            damageable.TakeDamage(ProvideDamage());
    }
}