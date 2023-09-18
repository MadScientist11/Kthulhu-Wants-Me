using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAttack : Attack
    {

        public bool IsAttacking => _isAttacking;
        
        protected override float BaseDamage => _enemy.EnemyStats.Stats[StatType.Damage];
        
        [SerializeField] private Enemy _enemy;
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private DamageModifier _damageModifier;

        private bool _isAttacking;
        private float _attackCooldown;
        
        [SerializeField] private float _attackRadius;
        [SerializeField] private float _tentacleGrabDamage;
        [SerializeField] private float _attackEffectiveDistance;
        [SerializeField] private float _attackCooldownTime;
        
        private void Update()
        {
            _attackCooldown -= Time.deltaTime;
        }

        protected override void OnAttack()
        {
            if (!PhysicsUtility.HitFirst(transform, AttackStartPoint(), _attackRadius,
                    LayerMasks.PlayerMask, out Transform player))
                return;

            ApplyDamage(to: player.GetComponent<IDamageable>());
            _damageModifier?.ApplyTo(player.GetComponent<IEffectReceiver>());
        }

        protected override void OnAttackEnd()
        {
            _isAttacking = false;
            _attackCooldown = _attackCooldownTime;
        }

        public void PerformAttack()
        {
            _isAttacking = true;
            _tentacleAnimator.PlayAttack();
        }

        public bool CanAttack()
        {
            return !_isAttacking && _attackCooldown <= 0;
        }
        
        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * _attackEffectiveDistance;
        }
    }
}