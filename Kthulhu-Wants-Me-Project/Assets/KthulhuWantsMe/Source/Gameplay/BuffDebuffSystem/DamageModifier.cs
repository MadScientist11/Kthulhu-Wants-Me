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

        public void SetModifier(DamageModifierId damageModifierId)
        {
            _damageModifierId = damageModifierId;
        }

        private IBuffDebuff CreateEffect()
        {
            return _damageModifierId switch
            {
                DamageModifierId.None => null,
                DamageModifierId.Poison => _buffDebuffFactory.CreateEffect<PoisonDebuff>().Init(5, 5f, _effectVfxPrefab),
                DamageModifierId.Bleed => _buffDebuffFactory.CreateEffect<BleedDebuff>().Init(2, 10f, _effectVfxPrefab),
                DamageModifierId.Fire => _buffDebuffFactory.CreateEffect<FireDebuff>().Init(9,3,_effectVfxPrefab),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    
    }
}