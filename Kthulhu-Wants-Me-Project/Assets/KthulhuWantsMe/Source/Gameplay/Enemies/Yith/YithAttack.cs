using System.Collections;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;
using Time = UnityEngine.Time;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithAttack : Attack
    {
        protected override float BaseDamage => _enemy.EnemyStats.Stats[StatType.Damage];
        
        [SerializeField] private Enemy _enemy;
        [SerializeField] private MMFeedbacks _attackFeedback;
        [SerializeField] private FollowLogic _followLogic;

        private float _attackCooldown;
        private bool _isAttacking;


        private void Update()
        {
            _attackCooldown -= Time.deltaTime;
        }

        public void PerformAttack()
        {
            if (!PhysicsUtility.HitFirst(transform, 
                    AttackStartPoint(), 
                    .75f, 
                    LayerMasks.PlayerMask, 
                    out IDamageable damageable))
                return;
            
            _isAttacking = true;

            ApplyDamage(to: damageable);
            
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