﻿using System.Collections;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.Enemies.Minion;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithHealth : Health
    {
        public override float MaxHealth => enemyStatsContainer.EnemyStats.Stats[StatType.MaxHealth];

        [FormerlySerializedAs("_enemy")] [SerializeField] private EnemyStatsContainer enemyStatsContainer;
        [SerializeField] private YithAIBrain _yithAIBrain;
        [SerializeField] private Collider _collider;
        [SerializeField] private MovementMotor _movementMotor;
        [SerializeField] private MMFeedbacks _hitFeedbacks;

        [SerializeField] private YithAnimator _yithAnimator;
        [SerializeField] private YithAttack _yithAttack;
        
        private YithConfiguration _yithConfiguration;
        
        private void Start()
        {
            Revive();
            Died += HandleDeath;
            _yithConfiguration = (YithConfiguration)enemyStatsContainer.Config;
        }

        private void OnDestroy()
        {
            Died -= HandleDeath;
        }

        public override void TakeDamage(float damage, IDamageProvider damageProvider)
        {
            if (CurrentHealth <= 0)
                return;
            
            base.TakeDamage(damage, damageProvider);

            if (damageProvider.DamageDealer != null)
            {
                _movementMotor.AddVelocity(damageProvider.DamageDealer.forward * _yithConfiguration.Knockback, _yithConfiguration.KnockbackTime);
                _yithAIBrain.Stunned = true;
            }
            
            if(_hitFeedbacks != null)
                _hitFeedbacks.PlayFeedbacks();
            
            _yithAttack.ResetAttackState();
        }

        private void HandleDeath()
        {
            StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            //_movementMotor.Velocity = -transform.forward * 15f;
            _collider.enabled = false;
            GetComponent<IStoppable>().StopEntityLogic();
            yield return new WaitForSeconds(.2f);
            _yithAnimator.PlayDie();
            Destroy(gameObject, 2f);
        }
    }
}