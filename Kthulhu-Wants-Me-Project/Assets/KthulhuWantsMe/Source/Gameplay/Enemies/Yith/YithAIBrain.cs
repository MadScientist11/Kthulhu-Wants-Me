using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithAIBrain : MonoBehaviour
    {
        public bool BlockProcessing { get; set; }
        
        [SerializeField] private YithHealth _yithHealth;
        [SerializeField] private YithAttack _yithAttack;
        [SerializeField] private YithRageComboAbility _yithRageComboAbility;
        [SerializeField] private FollowLogic _followLogic;
        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;

        private float _attackDelayTime;
        private float _rageComboRandom;

        private YithConfiguration _yithConfiguration;

        private const float ComboAttackReavaluationTime = 5f;

        private PlayerFacade _player;

        [Inject]
        public void Construct(IGameFactory gameFactory, IRandomService randomService)
        {
            _player = gameFactory.Player;
            
            randomService.ProvideRandomValue(value => _rageComboRandom = value, ComboAttackReavaluationTime);
        }

        private void Start()
        {
            _followLogic.Init(_player.transform, Mathf.Infinity, 2.5f);
            _yithConfiguration = (YithConfiguration)_enemyStatsContainer.Config;
            _followLogic.FollowSpeed = Random.Range((int)_yithConfiguration.MoveSpeed.x, (int)_yithConfiguration.MoveSpeed.y);
        }

        private void Update() => 
            DecideStrategy();

        public void ResetState()
        {
        }

        private void DecideStrategy()
        {
            if(_yithHealth.IsDead || BlockProcessing)
                return;
            
            DecideMoveStrategy();
            DecideAttackStrategy();
        }

        private void DecideMoveStrategy()
        {
            if (ShouldFollow() || _yithRageComboAbility.InProcess)
                _followLogic.Follow();
            else
                _followLogic.StopFollowing();
        }
        
        private void DecideAttackStrategy()
        {
            if(_yithRageComboAbility.InProcess)
                return;
            
            if(_followLogic.TargetReached)
                UpdateAttackDelayCountdown();
            else
                ResetAttackDelayCountdown();


            if (ComboAttackConditionsFulfilled())
            {
                _yithRageComboAbility.PerformCombo();
            }

            if (CanDoBasicAttack())
            {
                _yithAttack.PerformAttack();
                ResetAttackDelayCountdown();
            }
        }

   

        private void UpdateAttackDelayCountdown() => 
            _attackDelayTime -= Time.deltaTime;

        private void ResetAttackDelayCountdown() => 
            _attackDelayTime = .75f;

        private bool AttackDelayCountdownIsUp() 
            => _attackDelayTime <= 0;

        private bool ShouldFollow() => 
            _followLogic.CanFollow() && !_followLogic.TargetReached;

        private bool CanDoBasicAttack()
        {
            return _followLogic.TargetReached
                   && _yithAttack.CanAttack() 
                   && AttackDelayCountdownIsUp();
        }

        private bool ComboAttackConditionsFulfilled()
            => CanDoComboAttack() && _rageComboRandom > 0.5f;
        
        private bool CanDoComboAttack()
        {
            return _followLogic.DistanceToTarget is < 6f and > 4f && _yithRageComboAbility.CanComboAttack() &&
                   NoObstaclesToPlayer();
        }

        private bool NoObstaclesToPlayer()
        {
            Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;
            bool anyEnemiesInThePath = Physics.Raycast(transform.position, directionToPlayer, 100, LayerMasks.EnemyMask);
            return !anyEnemiesInThePath;
        }
    }
}