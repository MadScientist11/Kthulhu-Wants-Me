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
        [SerializeField] private YithAnimator _yithAnimator;

        [SerializeField] private HUDNavigationElement _navigationElement;

        [SerializeField] private int _comboCount;

        private bool _isAttacking;
        private bool _oneAttackFinished;
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

            if (_isAttacking)
            {
                if (!PhysicsUtility.HitFirst(transform,
                        AttackStartPoint(),
                        .75f,
                        LayerMasks.PlayerMask,
                        out IDamageable damageable))
                    return;

                damageable.TakeDamage(10);
                
                ResetAttackState();
            }
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
            StartCoroutine(DoCombo());
        }

        private IEnumerator DoCombo()
        {
            _isAttacking = true;

            _yithAnimator.PlayStance(0);
            yield return new WaitForSeconds(_yithConfiguration.ComboAttackDelay);

            if (IsPlayerReachable())
            {
                Vector3 directionToTarget = (_target - transform.position).normalized;
                GetComponent<Collider>().enabled = false;
                _movementMotor.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                _movementMotor.AddVelocity(directionToTarget * _yithConfiguration.DashDistance, _yithConfiguration.ComboAttackDashTime);
                _yithAnimator.PlayAttack();
            }
            else
            {
                ResetAttackState();
            }
        }

        private void OnAttackFinished()
        {
            ResetAttackState();
        }

        private void ResetAttackState()
        {
            _yithAnimator.ResetAttack();
            _isAttacking = false;
            _comboAttackCooldown = _yithConfiguration.ComboAttackCooldown;
            _movementMotor.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            GetComponent<Collider>().enabled = true;
        }

        public bool CanComboAttack()
        {
            if (_isAttacking || _comboAttackCooldown > 0f)
            {
                return false;
            }

            return IsPlayerReachable();
        }

        private bool IsPlayerReachable()
        {
            Vector3 directionToTarget = (_gameFactory.Player.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(directionToTarget, transform.forward);

            if (dot < 0.8)
            {
                return false;
            }

            _target = _gameFactory.Player.transform.position;
            return true;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * 1.25f;
        }
    }
}