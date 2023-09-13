using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs
{
    public class BleedDebuff : IBuffDebuff, IUpdatableEffect, IDamageProvider
    {
        private float _damagePerSecond;
        private float _duration;

        private IDamageable _damageable;
        private IEffectReceiver _effectReceiver;
        private float _effectStartTime;

        private ParticleSystem _poisonVFXInstance;

        private IBuffDebuffService _buffDebuffService;
        private ParticleSystem _poisonVFXPrefab;

        [Inject]
        public void Construct(IBuffDebuffService buffDebuffService)
        {
            _buffDebuffService = buffDebuffService;
        }

        public BleedDebuff Init(float damagePerSecond, float duration, ParticleSystem poisonVFXPrefab)
        {
            _poisonVFXPrefab = poisonVFXPrefab;
            _duration = duration;
            _damagePerSecond = damagePerSecond;
            return this;
        }

        public void ApplyEffect(IEffectReceiver effectReceiver)
        {
            _effectReceiver = effectReceiver;
            if (effectReceiver.Transform.TryGetComponent(out IDamageable damageable))
            {
                _damageable = damageable;
            }

            _poisonVFXInstance = GameObject.Instantiate(_poisonVFXPrefab, _effectReceiver.Transform);
            _poisonVFXInstance.transform.position += Vector3.up;
            _effectStartTime = Time.realtimeSinceStartup;
        }

        public void UpdateEffect()
        {
            if (Time.realtimeSinceStartup > _effectStartTime + _duration)
            {
                _buffDebuffService.CancelEffect(this, _effectReceiver);
                return;
            }

            _damageable.TakeDamage(ProvideDamage(), this);
        }

        public float ProvideDamage() =>
            _damagePerSecond * Time.deltaTime;

        public void CancelEffect(IEffectReceiver effectReceiver)
        {
                GameObject.Destroy(_poisonVFXInstance.gameObject);
        }
    }
}