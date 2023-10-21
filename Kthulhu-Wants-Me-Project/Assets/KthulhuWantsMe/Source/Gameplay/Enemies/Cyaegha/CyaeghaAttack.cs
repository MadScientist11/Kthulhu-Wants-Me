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


        public AnimationCurve HeightCurve;

        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;
        [FormerlySerializedAs("_attackFeedback")] [SerializeField] private MMFeedbacks _attackPrepareFeedback;
        [SerializeField] private NavMeshAgent _cyaeghaNavMesh;
        
        private float _attackCooldown;
        private bool _isAttacking;
        
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
            StartCoroutine(DoAttack(lastPlayerPosition));
        }

        private IEnumerator DoAttack(Vector3 lastPlayerPosition)
        {
            _aiService.SomeonesAttacking = true;
            _attackPrepareFeedback?.PlayFeedbacks();
            yield return new WaitForSeconds(0.5f);

            Vector3 jumpStartPos = transform.position;
            Vector3 dest = lastPlayerPosition;
            _isAttacking = true;
            
            FaceTarget(lastPlayerPosition);
            bool damaged = false;

            for (float t = 0; t < 1; t += Time.deltaTime * 2f)
            {
                transform.position = Vector3.Lerp(jumpStartPos, dest, t)
                                     + Vector3.up * HeightCurve.Evaluate(t);

                if (TryDamage(.4f, out IDamageable player) && !damaged)
                {
                    ApplyDamage(to: player);
                    damaged = true;
                }
                yield return null;
            }
            
            if (TryDamage(.6f, out IDamageable damageable) && !damaged)
            {
                ApplyDamage(to: damageable);
            }

          

            _isAttacking = false;
            _aiService.SomeonesAttacking = false;

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
            Vector3 directionToPlayer = (_gameFactory.Player.transform.position - transform.position).normalized;
            Ray ray = new Ray(transform.position, directionToPlayer);
            if (DrawPhysics.SphereCast(ray, 0.25f,  out RaycastHit hit,10f, LayerMasks.EnemyMask))
            {
                Debug.Log($"Raycast success {hit.transform.name}");
                return false;
            }
            return !_isAttacking && _attackCooldown <= 0f;
        }


        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * 1.25f;
        }
    }
}