using System;
using System.Collections;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Player;
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

        private int _comboCount = 1;

        private bool _isAttacking;
        private bool _contactPhase;
        private float _comboAttackCooldown;

        private YithConfiguration _yithConfiguration;
        private NavMeshPath _navMeshPath;
        private Vector3 _target;

        private PlayerFacade _player;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }

        private void Start()
        {
            _yithConfiguration = (YithConfiguration)_enemyStatsContainer.Config;
            _navMeshPath = new();
        }

        private void Update()
        {
            _comboAttackCooldown -= Time.deltaTime;

            if (_isAttacking && _contactPhase)
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
            _movementMotor.Agent.angularSpeed = 60;

            _yithAnimator.PlayStance(0);
            _navigationElement.SwitchOn();
            yield return new WaitForSeconds(_yithConfiguration.ComboAttackDelay);
            _navigationElement.SwitchOff();

            if (IsPlayerReachable())
            {
                PrepareForAttack();
                Vector3 directionToTarget = (_target - transform.position).normalized;
                _movementMotor.AddVelocity(directionToTarget * _yithConfiguration.DashDistance, _yithConfiguration.ComboAttackDashTime);
                _yithAnimator.PlayAttack();
                _contactPhase = true;
            }
            else
            {
                ResetAttackState();
            }
        }

        private void OnAttackFinished()
        {
            _comboCount++;
            
            if(_comboCount > _yithConfiguration.ComboCount)
                ResetAttackState();
            else
            {
                if (!_isAttacking)
                {
                    ResetAttackState();
                    return;
                }
                
                if(_comboCount == 2)
                    _secondComboAttack = StartCoroutine(SecondAttackCombo());
                else if (_comboCount == 3)
                    _thirdComboAttack = StartCoroutine(ThirdAttackCombo());
                
            }
        }

        private Coroutine _secondComboAttack;
        private Coroutine _thirdComboAttack;

        private IEnumerator SecondAttackCombo()
        {
            _yithAnimator.PlayStance(1);
            yield return new WaitForSeconds(_yithConfiguration.DelayBetweenComboAttacks);
            
            GetComponent<Collider>().enabled = false;
            _movementMotor.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            Vector3 directionToTarget = transform.forward;
            _movementMotor.AddVelocity(directionToTarget * _yithConfiguration.DashDistance, _yithConfiguration.ComboAttackDashTime);
            _yithAnimator.PlayAttack();
            _contactPhase = true;
        }
        
        private IEnumerator ThirdAttackCombo()
        {
            _yithAnimator.PlayStance(0);
            yield return new WaitForSeconds(_yithConfiguration.DelayBetweenComboAttacks);
            
            GetComponent<Collider>().enabled = false;
            _movementMotor.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            Vector3 directionToTarget = transform.forward;
            _movementMotor.AddVelocity(directionToTarget * _yithConfiguration.DashDistance, _yithConfiguration.ComboAttackDashTime);
            _yithAnimator.PlayAttack();
            _contactPhase = true;
        }

        private void ResetAttackState()
        {
            _yithAnimator.ResetAttack();
            _isAttacking = false;
            _comboAttackCooldown = _yithConfiguration.ComboAttackCooldown;
            _movementMotor.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            GetComponent<Collider>().enabled = true;
            
            if(_secondComboAttack != null) StopCoroutine(_secondComboAttack);
            if(_thirdComboAttack != null) StopCoroutine(_thirdComboAttack);
            _comboCount = 1;
            _contactPhase = false;
            _movementMotor.Agent.angularSpeed = 720;
        }

        private void PrepareForAttack()
        {
            _target = _player.transform.position;
            GetComponent<Collider>().enabled = false;
            _movementMotor.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
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
            Vector3 directionToTarget = (_player.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(directionToTarget, transform.forward);

            if (dot < 0.7)
            {
                return false;
            }

            return true;
        }

        private Vector3 AttackStartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * 1.25f;
        }
    }
}