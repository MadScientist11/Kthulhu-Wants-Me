using System;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public class DamageModifier : MonoBehaviour
    {
        [SerializeField] private DamageModifierId _damageModifierId;
        
        [FormerlySerializedAs("_poisonVFXPrefab")] [SerializeField] private ParticleSystem _effectVfxPrefab;

        private IBuffDebuffService _buffDebuffService;
        private IBuffDebuffFactory _buffDebuffFactory;
        private float _damagePerSecond;
        private float _duration;

        [Inject]
        public void Construct(IBuffDebuffService buffDebuffService, IBuffDebuffFactory buffDebuffFactory)
        {
            _buffDebuffFactory = buffDebuffFactory;
            _buffDebuffService = buffDebuffService;
        }

        public void ApplyTo(IEffectReceiver effectReceiver)
        {
            if(_damageModifierId == DamageModifierId.None)
                return;
            
            _buffDebuffService.ApplyEffect(CreateEffect(), effectReceiver);
        }

        public void SetModifier(DamageModifierId damageModifierId, float damagePerSecond = 9, float duration = 3)
        {
            _duration = duration;
            _damagePerSecond = damagePerSecond;
            _damageModifierId = damageModifierId;
        }

        private IBuffDebuff CreateEffect()
        {
            return _damageModifierId switch
            {
                DamageModifierId.None => null,
                DamageModifierId.Poison => _buffDebuffFactory.CreateEffect<PoisonDebuff>().Init(5, 5f, _effectVfxPrefab),
                DamageModifierId.Bleed => _buffDebuffFactory.CreateEffect<BleedDebuff>().Init(2, 10f, _effectVfxPrefab),
                DamageModifierId.Fire => _buffDebuffFactory.CreateEffect<FireDebuff>().Init(_damagePerSecond,_duration,_effectVfxPrefab),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    
    }
}