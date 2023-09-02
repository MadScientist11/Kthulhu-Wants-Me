﻿using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAttack : MonoBehaviour, IDamageProvider, IDamageSource
    {
        public Transform DamageSourceObject => transform;

        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private TentacleAIBrain _tentacleAIBrain;

        private TentacleConfiguration _tentacleConfig;

        [Inject]
        public void Construct(IDataProvider dataProvider)
        {
            _tentacleConfig = dataProvider.TentacleConfig;
        }

        public void PerformAttack()
        {
            _tentacleAIBrain.IsAttacking = true;
            _tentacleAnimator.PlayAttack();
        }

        public float ProvideDamage()
        {
            return 25;
        }

        private void OnAttack()
        {
            if (!PhysicsUtility.HitFirst(transform, AttackStartPoint(), _tentacleConfig.AttackRadius, out IDamageable hitObject))
                return;

            hitObject.TakeDamage(ProvideDamage());
        }

        private void OnAttackEnd()
        {
            _tentacleAIBrain.IsAttacking = false;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * _tentacleConfig.AttackEffectiveDistance;
        }
    }
}