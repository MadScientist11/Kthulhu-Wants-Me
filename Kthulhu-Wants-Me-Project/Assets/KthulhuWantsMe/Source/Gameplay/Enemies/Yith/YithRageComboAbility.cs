using System;
using System.Collections;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Infrastructure.Services;
using MoreMountains.Feedbacks;
using SickscoreGames.HUDNavigationSystem;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithRageComboAbility : MonoBehaviour, IAbility
    {
        public bool InProcess => _isAttacking;

        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;
        [SerializeField] private MMFeedbacks _comboFeedback;
        [SerializeField] private MMFeedbacks _comboChargeFeedback;
        [SerializeField] private MovementMotor _movementMotor;

        [SerializeField] private HUDNavigationElement _navigationElement;

        [SerializeField] private int _comboCount;

        private bool _isAttacking;
        private float _comboAttackCooldown;

        private YithConfiguration _yithConfiguration;
        private NavMeshPath _navMeshPath;
        private Vector3 _target;

        private IGameFactory _gameFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        private void Start()
        {
            _yithConfiguration = (YithConfiguration)_enemyStatsContainer.Config;
            _navMeshPath = new();
        }

        private void Update()
        {
            _comboAttackCooldown -= Time.deltaTime;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_target, .5f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }

        public void PerformCombo()
        {
            StartCoroutine(ComboAttack());
        }

        private IEnumerator ComboAttack()
        {
            _isAttacking = true;

            _navigationElement.SwitchOn();
            _comboChargeFeedback?.PlayFeedbacks();
            yield return new WaitForSeconds(_yithConfiguration.ComboAttackDelay);
            _navigationElement.SwitchOff();


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
            Vector3 directionToTarget = (_target - transform.position).normalized;
            _movementMotor.AddVelocity(directionToTarget * _yithConfiguration.DashDistance, _yithConfiguration.ComboAttackDashSpeed, OnDashed);

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
            if (_isAttacking || _comboAttackCooldown > 0f)
            {
                return false;
            }
            
            Vector3 directionToTarget = (_gameFactory.Player.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(directionToTarget, transform.forward);
            
            if (dot < 0.8)
            {
                return false;
            }

            //Vector3 randomPoint = _gameFactory.Player.transform.position + Random.insideUnitSphere * .5f;
            bool sampleSuccess = NavMesh.SamplePosition(_gameFactory.Player.transform.position, out NavMeshHit hit, 0.25f, NavMesh.AllAreas);
            if (!sampleSuccess)
            {
                Debug.Log("Sample failed");
                return false;
            }

            _movementMotor.Agent.CalculatePath(hit.position, _navMeshPath);
            if (_navMeshPath.status == NavMeshPathStatus.PathComplete &&
                !NavMesh.Raycast(transform.position, hit.position, out NavMeshHit _,
                    NavMesh.AllAreas))
            {
                _target = hit.position;
                return true;

            }

            return false;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * 1.25f;
        }
    }
}