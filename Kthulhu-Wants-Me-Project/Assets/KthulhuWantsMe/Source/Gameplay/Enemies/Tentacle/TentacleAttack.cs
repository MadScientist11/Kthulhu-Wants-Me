using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAttack : Attack
    {
        protected override float BaseDamage => _tentacleConfig.BaseDamage;
        
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleAIBrain _tentacleAIBrain;
        
        private TentacleConfiguration _tentacleConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider) => 
            _tentacleConfig = dataProvider.TentacleConfig;

        protected override void OnAttack()
        {
            if (!PhysicsUtility.HitFirst(transform, AttackStartPoint(), _tentacleConfig.AttackRadius,
                    LayerMasks.PlayerMask, out IDamageable damageable))
                return;

            ApplyDamage(to: damageable);
        }

        protected override void OnAttackEnd()
        {
            _tentacleAIBrain.IsAttacking = false;
        }

        public void PerformAttack()
        {
            _tentacleAIBrain.IsAttacking = true;
            _tentacleAnimator.PlayAttack();
        }
        
        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * _tentacleConfig.AttackEffectiveDistance;
        }
    }
}