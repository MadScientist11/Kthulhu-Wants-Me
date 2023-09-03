using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Minion
{
    public class MinionAttack : Attack
    {
        protected override float BaseDamage => _minionConfiguration.BaseDamage;
        
        [SerializeField] private MinionAIBrain _minionAIBrain;

        private float _attackCooldown;
        
        private MinionConfiguration _minionConfiguration;

        [Inject]
        public void Construct(IDataProvider dataProvider) => 
            _minionConfiguration = dataProvider.MinionConfig;
        
        private void Update()
        {
            _attackCooldown -= Time.deltaTime;
            if (_attackCooldown <= 0) 
                _minionAIBrain.AttackCooldownReached = true;
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
            _attackCooldown = _minionConfiguration.AttackCooldownTime;
            _minionAIBrain.AttackCooldownReached = false;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * .25f;
        }
    }
}