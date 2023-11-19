using System;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.StaterResetter;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public interface IHealable
    {
        void Heal(float amount);
    }

    public abstract class Health : MonoBehaviour, IDamageable, IHealable, IStateReset
    {
        public Transform Transform => transform;
        public bool IsDead { get; private set; }
        public abstract float MaxHealth { get; }

        public virtual float CurrentHealth
        {
            get => _currentHealth;
            protected set
            {
                if(IsDead)
                    return;

                float newHealth = Mathf.Clamp(value, 0, MaxHealth);
                if (newHealth < _currentHealth)
                {
                    TookDamage?.Invoke();
                }

                _currentHealth = newHealth;

                Changed?.Invoke(_currentHealth);

                if (_currentHealth == 0)
                {
                    Died?.Invoke();
                    Died = null;
                    IsDead = true;
                }
            }
        }

        public event Action<float> Changed;
        public event Action TookDamage;
        public event Action Died;

        private float _currentHealth;


        public void ResetState() => 
            Revive();

        public virtual void TakeDamage(float damage, IDamageProvider damageProvider) =>
            CurrentHealth -= damage;

        public virtual void TakeDamage(float damage) => 
            CurrentHealth -= damage;

        public virtual void Heal(float amount) =>
            CurrentHealth += amount;
        
        protected void RaiseTookDamageEvent() => TookDamage?.Invoke();

        protected void RaiseHealthChangedEvent(float newValue) => Changed?.Invoke(newValue);

        protected void RaiseDiedEvent() => Died?.Invoke();

        public void Revive()
        {
            IsDead = false;
            CurrentHealth = MaxHealth;
        }
    }
}