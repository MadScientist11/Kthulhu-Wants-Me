﻿using System;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem.BuffsDebuffs;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public class DamageModifier : MonoBehaviour
    {
        [SerializeField] private DamageModifierId _damageModifierId;
        
        [SerializeField] private ParticleSystem _poisonVFXPrefab;

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
            _buffDebuffService.ApplyEffect(CreateEffect(), effectReceiver);
        }

        private IBuffDebuff CreateEffect()
        {
            return _damageModifierId switch
            {
                DamageModifierId.None => null,
                DamageModifierId.Poison => _buffDebuffFactory.CreateEffect<PoisonDebuff>().Init(5, 5f, _poisonVFXPrefab),
                DamageModifierId.Bleed => _buffDebuffFactory.CreateEffect<BleedDebuff>().Init(2, 10f, _poisonVFXPrefab),
                DamageModifierId.Fire => _buffDebuffFactory.CreateEffect<FireDebuff>().Init(9,5,_poisonVFXPrefab),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    
    }
}