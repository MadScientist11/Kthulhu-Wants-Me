using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAttack : Attack
    {
        public bool IsAttacking => _isAttacking;
        
        protected override float BaseDamage => enemyStatsContainer.EnemyStats.Stats[StatType.BaseDamage];
        
        [FormerlySerializedAs("_enemy")] [SerializeField] private EnemyStatsContainer enemyStatsContainer;
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleFacade _tentacleFacade;
        [SerializeField] private DamageModifier _damageModifier;

        private bool _isAttacking;
        private float _attackCooldown;
        
        
        private TentacleConfiguration _tentacleConfiguration;

        private void Start()
        {
            _tentacleConfiguration = (TentacleConfiguration)enemyStatsContainer.Config;
        }

        private void Update()
        {
            _attackCooldown -= Time.deltaTime;
        }

        protected override void OnAttack()
        {
            if (!PhysicsUtility.HitFirst(transform, AttackStartPoint(), _tentacleConfiguration.AttackRadius,
                    LayerMasks.PlayerMask, out Transform player) || _tentacleFacade.TentacleAIBrain.BlockProcessing)
                return;
            
            
            ApplyDamage(to: player.GetComponent<IDamageable>());
            _damageModifier?.ApplyTo(player.GetComponent<IEffectReceiver>());
        }

        protected override void OnAttackEnd()
        {
            _isAttacking = false;
            _attackCooldown = _tentacleConfiguration.AttackCooldownTime;
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
                   transform.forward * _tentacleConfiguration.AttackEffectiveDistance;
        }
    }
}