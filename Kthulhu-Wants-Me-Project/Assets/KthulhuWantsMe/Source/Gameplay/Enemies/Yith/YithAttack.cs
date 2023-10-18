using System;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;
using Time = UnityEngine.Time;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithAttack : Attack
    {
        protected override float BaseDamage => enemyStatsContainer.EnemyStats.Stats[StatType.BaseDamage];
        
        [FormerlySerializedAs("_enemy")] [SerializeField] private EnemyStatsContainer enemyStatsContainer;
        [SerializeField] private MMFeedbacks _attackFeedback;

        private float _attackCooldown;
        private bool _isAttacking;
        
        private YithConfiguration _yithConfiguration;

        private void Start()
        {
            _yithConfiguration = (YithConfiguration)enemyStatsContainer.Config;
        }


        private void Update()
        {
            _attackCooldown -= Time.deltaTime;
        }

        public void PerformAttack()
        {
            if (!PhysicsUtility.HitFirst(transform, 
                    AttackStartPoint(), 
                    _yithConfiguration.AttackRadius, 
                    LayerMasks.PlayerMask, 
                    out IDamageable damageable))
                return;
            
            _isAttacking = true;

            ApplyDamage(to: damageable);
            _attackFeedback?.PlayFeedbacks();
            
            _isAttacking = false;
            _attackCooldown = 1f;
        }

        public bool CanAttack()
        {
            return !_isAttacking && _attackCooldown <= 0f;
        }
        
   
        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * 1.25f;
        }
    }
}