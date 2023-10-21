using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using VContainer;
using Vertx.Debugging;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaAttack : Attack
    {
        protected override float BaseDamage => _enemyStatsContainer.EnemyStats.Stats[StatType.BaseDamage];
        public bool IsAttacking => _isAttacking;

        public AnimationCurve HeightCurve;
        public AnimationCurve LandingCurve;

        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;

        [FormerlySerializedAs("_attackFeedback")] [SerializeField]
        private MMFeedbacks _attackPrepareFeedback;

        [SerializeField] private NavMeshAgent _cyaeghaNavMesh;

        private float _attackCooldown;
        private bool _isAttacking;
        private Coroutine _attackCoroutine;

        private CyaeghaConfiguration _cyaeghaConfiguration;
        private IGameFactory _gameFactory;
        private IAIService _aiService;

        [Inject]
        public void Construct(IGameFactory gameFactory, IAIService aiService)
        {
            _aiService = aiService;
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            _cyaeghaConfiguration = (CyaeghaConfiguration)_enemyStatsContainer.Config;
        }

        private void Update()
        {
            _attackCooldown -= Time.deltaTime;
        }

        public void PerformAttack(Vector3 lastPlayerPosition)
        {
            _attackCoroutine = StartCoroutine(DoAttack(lastPlayerPosition));
        }

        public void StopAttack()
        {
            if (_isAttacking)
            {
                _aiService.SomeonesAttacking = false;
                _cyaeghaNavMesh.enabled = true;
                StopCoroutine(_attackCoroutine);
                _attackCooldown = _cyaeghaConfiguration.AttackCooldown;
            }
        }

        private IEnumerator DoAttack(Vector3 lastPlayerPosition)
        {
            _aiService.SomeonesAttacking = true;
            _cyaeghaNavMesh.enabled = false;
            _attackPrepareFeedback?.PlayFeedbacks();
            yield return new WaitForSeconds(0.5f);

            Vector3 jumpStartPos = transform.position;
            Vector3 dest = lastPlayerPosition;
            _isAttacking = true;

            bool damaged = false;

            Vector3 damagePosition = Vector3.zero;

            for (float t = 0; t < 1; t += Time.deltaTime * 2f)
            {
                transform.position = Vector3.Lerp(jumpStartPos, dest, t)
                                     + Vector3.up * (HeightCurve.Evaluate(t) * 2f);
                
                if ( t > 0.25f && TryDamage(.4f, out IDamageable player))
                {
                    ApplyDamage(to: player);
                    damagePosition = transform.position;
                    damaged = true;
                    break;
                }

                yield return null;
            }

            if (damaged)
            {
                yield return new WaitForSeconds(0.05f);

                Vector3 desiredXZ = damagePosition - transform.forward * .75f;
                bool sampleSuccess =
                    NavMesh.SamplePosition(desiredXZ, out NavMeshHit hit, 2f, NavMesh.AllAreas);

                if (sampleSuccess)
                {
                    for (float t = 0; t < 1; t += Time.deltaTime * 5f)
                    {
                        transform.position = Vector3.Lerp(damagePosition, hit.position, LandingCurve.Evaluate(t));
                        yield return null;
                    }
                }
            }

            _isAttacking = false;
            _aiService.SomeonesAttacking = false;
            _cyaeghaNavMesh.enabled = true;

            _attackCooldown = _cyaeghaConfiguration.AttackCooldown;
        }

        private bool TryDamage(float damageRadius, out IDamageable damageable)
        {
            if (PhysicsUtility.HitFirst(transform,
                    transform.position,
                    damageRadius,
                    LayerMasks.PlayerMask,
                    out IDamageable dmg))
            {
                damageable = dmg;
                return true;
            }

            damageable = null;
            return false;
        }

        private void FaceTarget(Vector3 destination)
        {
            Vector3 lookPos = destination - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 3);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
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