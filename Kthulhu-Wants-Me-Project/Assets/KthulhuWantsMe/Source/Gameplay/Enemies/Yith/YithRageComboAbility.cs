using System;
using System.Collections;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithRageComboAbility : MonoBehaviour, IAbility
    {
        public bool InProcess => _isAttacking;
        
        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;
        [SerializeField] private MMFeedbacks _comboFeedback;
        [SerializeField] private MMFeedbacks _comboChargeFeedback;
        [SerializeField] private MovementMotor _movementMotor;

        [SerializeField] private int _comboCount;
        
        private bool _isAttacking;
        private float _comboAttackCooldown;

        private YithConfiguration _yithConfiguration;

        private void Start()
        {
            _yithConfiguration = (YithConfiguration)_enemyStatsContainer.Config;
        }

        private void Update()
        {
            _comboAttackCooldown -= Time.deltaTime;
        }

        public void PerformCombo()
        {
            StartCoroutine(ComboAttack());
        }
        
        private IEnumerator ComboAttack()
        {
            _isAttacking = true;
            
            _comboChargeFeedback?.PlayFeedbacks();
            yield return new WaitForSeconds(.5f);
            
            for (int i = 0; i < _comboCount; i++)
            {
                PerformAttack();
                yield return new WaitForSeconds(_yithConfiguration.DelayBetweenComboAttacks);
            }
            
            
            _isAttacking = false;
            _comboAttackCooldown = _yithConfiguration.ComboAttackCooldown;
        }

        private void PerformAttack()
        {
            _movementMotor.AddVelocity(transform.forward * 15, .5f, OnDashed);

            void OnDashed()
            {
                _comboFeedback.PlayFeedbacks();
            
                if (!PhysicsUtility.HitFirst(transform, 
                        AttackStartPoint(), 
                        .75f, 
                        LayerMasks.PlayerMask, 
                        out IDamageable damageable))
                    return;
            

                damageable.TakeDamage(10);
            }
        }
        
        public bool CanComboAttack()
        {
            return !_isAttacking && _comboAttackCooldown <= 0f;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * 1.25f;
        }
    }
}