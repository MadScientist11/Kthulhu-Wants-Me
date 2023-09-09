﻿using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    public class MinionAttack : Attack
    {
        protected override float BaseDamage => _cyaeghaConfiguration.BaseDamage;
        
        [FormerlySerializedAs("_minionAIBrain")] [SerializeField] private YithAIBrain yithAIBrain;

        private float _attackCooldown;
        
        private CyaeghaConfiguration _cyaeghaConfiguration;

        [Inject]
        public void Construct(IDataProvider dataProvider) => 
            _cyaeghaConfiguration = dataProvider.CyaeghaConfig;
        
        private void Update()
        {
            _attackCooldown -= Time.deltaTime;
           // if (_attackCooldown <= 0) 
           //     yithAIBrain.AttackCooldownReached = true;
        }

        public void PerformAttack()
        {
            if (!PhysicsUtility.HitFirst(transform, 
                    AttackStartPoint(), 
                    1f, 
                    LayerMasks.PlayerMask, 
                    out IDamageable damageable))
                return;

            ResetCooldown();
            ApplyDamage(to: damageable);
        }
        
        private void ResetCooldown()
        {
            _attackCooldown = _cyaeghaConfiguration.AttackCooldownTime;
          //  yithAIBrain.AttackCooldownReached = false;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * .25f;
        }
    }
}