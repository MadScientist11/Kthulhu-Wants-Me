using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs
{
    public class FireBuff : IBuffDebuff, IUpdatableEffect
    {
        private DamageModifier _damageModifier;
        private IEffectReceiver _effectReceiver;
        private float _effectStartTime;
        private float _duration;

        private IBuffDebuffService _buffDebuffService;

        [Inject]
        public void Construct(IBuffDebuffService buffDebuffService)
        {
            _buffDebuffService = buffDebuffService;
        }

        public FireBuff Init(float duration)
        {
            _duration = duration;
            return this;
        }

        public void ApplyEffect(IEffectReceiver effectReceiver)
        {
            _effectReceiver = effectReceiver;

            if (effectReceiver.Transform.TryGetComponent(out DamageModifier damageModifier))
            {
                _damageModifier = damageModifier;
                _damageModifier.SetModifier(DamageModifierId.Fire);
            }

            _effectStartTime = Time.realtimeSinceStartup;
        }

        public void UpdateEffect()
        {
            if (Time.realtimeSinceStartup > _effectStartTime + _duration)
            {
                _buffDebuffService.CancelEffect(this, _effectReceiver);
                return;
            }
        }

        public void CancelEffect(IEffectReceiver effectReceiver)
        {
            _damageModifier.SetModifier(DamageModifierId.None);
        }
    }
}