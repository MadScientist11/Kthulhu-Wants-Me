using System.Collections;
using DG.Tweening;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaAttack : Attack
    {
        protected override float BaseDamage => _enemyStatsContainer.EnemyStats.Stats[StatType.BaseDamage];
        public bool IsAttacking => _isAttacking;


        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;

        [FormerlySerializedAs("_attackFeedback")] [SerializeField]
        private MMFeedbacks _attackPrepareFeedback;

        [SerializeField] private NavMeshAgent _cyaeghaNavMesh;

        [Header("Jump")] [SerializeField] private Ease _jumpEasing;

        [SerializeField] private float _delayBeforeJump = 0.75f;
        [SerializeField] private float _jumpSpeed = 2;
        [SerializeField] private float _jumpHeight = 2;

        [Header("Landing")] [SerializeField] private Ease _landEasing;
        [SerializeField] private float _landingSpeed = 5;


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
                ResetAttackState();
                StopCoroutine(_attackCoroutine);
            }
        }

        private IEnumerator DoAttack(Vector3 lastPlayerPosition)
        {
            SetAttackState();

            _attackPrepareFeedback?.PlayFeedbacks();
            yield return new WaitForSeconds(_delayBeforeJump);

            Vector3 jumpStartPos = transform.position;
            Vector3 dest = lastPlayerPosition;

            bool damaged = false;

            Vector3 damagePosition = Vector3.zero;

            for (float t = 0; t < 1; t += Time.deltaTime * _jumpSpeed)
            {
                transform.position = Vector3.Lerp(jumpStartPos, dest, t)
                                     + Vector3.up * DOVirtual.EasedValue(0, _jumpHeight, t, _jumpEasing);

                if (t > 0.25f && TryDamage(.4f, out IDamageable player))
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
                    for (float t = 0; t < 1; t += Time.deltaTime * _landingSpeed)
                    {
                        transform.position = Vector3.Lerp(damagePosition, hit.position,
                            DOVirtual.EasedValue(0, 1, t, _landEasing));
                        yield return null;
                    }
                }
            }
            else
            {
                Vector3 desiredXZ = transform.position + transform.forward * 1f;
                bool sampleSuccess =
                    NavMesh.SamplePosition(desiredXZ, out NavMeshHit hit, 3f, NavMesh.AllAreas);

                if (sampleSuccess)
                {
                    Vector3 startPosition = transform.position;
                    for (float t = 0; t < 1; t += Time.deltaTime * _landingSpeed)
                    {
                        transform.position = Vector3.Lerp(startPosition, hit.position,
                            DOVirtual.EasedValue(0, 1, t, _jumpEasing));

                        yield return null;
                    }
                }
            }

            ResetAttackState();
        }

        private void SetAttackState()
        {
            _aiService.SomeonesAttacking = true;
            _cyaeghaNavMesh.enabled = false;
            _isAttacking = true;
        }

        private void ResetAttackState()
        {
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }


        public bool CanAttack()
        {
            return !_isAttacking && _attackCooldown <= 0f;
        }
    }
}