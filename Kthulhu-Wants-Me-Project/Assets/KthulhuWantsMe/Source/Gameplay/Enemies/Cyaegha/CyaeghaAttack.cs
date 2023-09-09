using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Infrastructure.Services;
using MoreMountains.Feedbacks;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaAttack : Attack
    {
        protected override float BaseDamage => _cyaeghaConfiguration.BaseDamage;

        [SerializeField] private MMFeedbacks _attackFeedback;
        [SerializeField] private FollowLogic _followLogic;

        private float _attackCooldown;
        private bool _isAttacking;


        private CyaeghaConfiguration _cyaeghaConfiguration;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _cyaeghaConfiguration = dataProvider.CyaeghaConfig;
        }

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
            _attackCooldown = _cyaeghaConfiguration.AttackCooldownTime;
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