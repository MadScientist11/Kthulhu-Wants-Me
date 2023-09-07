using System;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Entity
{
    public abstract class Attack : MonoBehaviour, IDamageProvider
    {
        protected abstract float BaseDamage { get; }

        public virtual float ProvideDamage() =>
            BaseDamage;
        
        protected virtual void OnWindUpPhase()
        {
        }
        
        protected virtual void OnContactPhase()
        {
        }
        
        protected virtual void OnRecoveryPhase()
        {
        }

        protected virtual void OnAttackEnd()
        {
        }

        [Obsolete]
        protected virtual void OnAttack()
        {
        }

        protected void ApplyDamage(IDamageable to)
        {
            to.TakeDamage(ProvideDamage());
        }
    }
}