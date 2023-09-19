using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
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

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaAttack : Attack
    {
        protected override float BaseDamage => _enemy.EnemyStats.Stats[StatType.Damage];


        public AnimationCurve HeightCurve;

        [SerializeField] private Enemy _enemy;
        [SerializeField] private NavMeshAgent _cyaeghaNavMesh;
        
        private float _attackCooldown;
        private bool _isAttacking;
    
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
            Vector3 jumpStartPos = transform.position;
            Vector3 dest = lastPlayerPosition;
            _isAttacking = true;
            
            FaceTarget(lastPlayerPosition);

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

            if (NavMesh.SamplePosition(lastPlayerPosition, out NavMeshHit hit,1f,_cyaeghaNavMesh.areaMask))
            {
                _cyaeghaNavMesh.Warp(hit.position);
            }

            _isAttacking = false;
            _attackCooldown = 1f;
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