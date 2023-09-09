using System.Collections;
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


        public AnimationCurve HeightCurve;


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

        public void PerformAttack(Vector3 lastPlayerPosition)
        {
            StartCoroutine(DoAttack(lastPlayerPosition));

            // ApplyDamage(to: damageable);
        }

        private IEnumerator DoAttack(Vector3 lastPlayerPosition)
        {
            Vector3 jumpStartPos = transform.position;
            Vector3 dest = lastPlayerPosition;
            _isAttacking = true;

            for (float t = 0; t < 1; t += Time.deltaTime * 2f)
            {
                transform.position = Vector3.Lerp(jumpStartPos, dest, t)
                                     + Vector3.up * HeightCurve.Evaluate(t);
                yield return null;
            }

            if (PhysicsUtility.HitFirst(transform,
                    AttackStartPoint(),
                    .75f,
                    LayerMasks.PlayerMask,
                    out IDamageable damageable))
            {
                ApplyDamage(to: damageable);
            }

            //NavMesh SamplePosition?

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