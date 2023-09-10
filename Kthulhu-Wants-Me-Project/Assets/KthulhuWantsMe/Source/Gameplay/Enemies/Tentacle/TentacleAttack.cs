using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{    
    public enum DamageModifierId
    {
        None = 0,
        Poison = 1,
        Bleed = 2,
    }

    public class TentacleAttack : Attack
    {

        public bool IsAttacking => _isAttacking;
        
        protected override float BaseDamage => _tentacleConfig.BaseDamage;
        
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleAIBrain _tentacleAIBrain;
        [SerializeField] private DamageModifier _damageModifier;

        private bool _isAttacking;
        private float _attackCooldown;
        
        
        private TentacleConfiguration _tentacleConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider) => 
            _tentacleConfig = dataProvider.TentacleConfig;

        private void Update()
        {
            _attackCooldown -= Time.deltaTime;
        }

        protected override void OnAttack()
        {
            if (!PhysicsUtility.HitFirst(transform, AttackStartPoint(), _tentacleConfig.AttackRadius,
                    LayerMasks.PlayerMask, out Transform player))
                return;

            ApplyDamage(to: player.GetComponent<IDamageable>());
            _damageModifier?.ApplyTo(player.GetComponent<IEffectReceiver>());
        }

        protected override void OnAttackEnd()
        {
            _isAttacking = false;
            _attackCooldown = _tentacleConfig.AttackCooldown;
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
                   transform.forward * _tentacleConfig.AttackEffectiveDistance;
        }
    }
}