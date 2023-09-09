using System;
using System.Collections;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithRageComboAbility : MonoBehaviour, IAbility
    {
        public bool InProcess => _isAttacking;
        
        [SerializeField] private FollowLogic _followLogic;
        [SerializeField] private MMFeedbacks _comboFeedback;

        private bool _isAttacking;
        private float _comboAttackCooldown;

        private const float ComboAttackCooldownTime = 10f;

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
            _followLogic.FollowSpeed += 6;
            _isAttacking = true;

            PerformAttack();
            _comboFeedback.PlayFeedbacks();
            yield return new WaitForSeconds(0.75f);
            PerformAttack();
            _comboFeedback.PlayFeedbacks();
            yield return new WaitForSeconds(0.75f);
            PerformAttack();
            _comboFeedback.PlayFeedbacks();
            yield return new WaitForSeconds(0.75f);
            
            _isAttacking = false;
            _comboAttackCooldown = ComboAttackCooldownTime;
            _followLogic.FollowSpeed -= 6;
        }

        private void PerformAttack()
        {
            if (!PhysicsUtility.HitFirst(transform, 
                    AttackStartPoint(), 
                    .75f, 
                    LayerMasks.PlayerMask, 
                    out IDamageable damageable))
                return;
            

            damageable.TakeDamage(10);
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