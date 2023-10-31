using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithAIBrain : MonoBehaviour, IEnemyAIBrain
    {
        public bool BlockProcessing { get; set; }
        public bool Stunned { get; set; }

        [SerializeField] private YithHealth _yithHealth;
        [SerializeField] private YithAttack _yithAttack;
        [SerializeField] private YithRageComboAbility _yithRageComboAbility;
        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;

        [SerializeField] private FollowPlayer _followPlayerBehaviour;
        [SerializeField] private Patrol _patrolBehaviour;

        private float _attackDelayTime;
        private float _rageComboRandom;
        private float _reconsiderationTime;
        private float _lastStunTime;

        private YithConfiguration _yithConfiguration;

        private const float ComboAttackReavaluationTime = 5f;

        private PlayerFacade _player;
        private IAIService _aiService;

        [Inject]
        public void Construct(IGameFactory gameFactory, IAIService aiService, IRandomService randomService)
        {
            _aiService = aiService;
            _player = gameFactory.Player;
            
            randomService.ProvideRandomValue(value => _rageComboRandom = value, ComboAttackReavaluationTime);
        }

        private void Start()
        {
            _yithConfiguration = (YithConfiguration)_enemyStatsContainer.Config;
            
        }

        private void Update()
        {
            if (Stunned)
            {
                _lastStunTime = Time.time;
                Stunned = false;
            }
            
            DecideStrategy();
        }

        public void ResetState()
        {
        }

        private void DecideStrategy()
        {
            if(_yithHealth.IsDead || BlockProcessing)
                return;

            if (_lastStunTime + 2f > Time.time)
            {
                return;
            }

            DecideMoveStrategy();
            DecideAttackStrategy();
        }

        private void DecideMoveStrategy()
        {
            if (_yithRageComboAbility.InProcess)
            {
                return;
            }
            
            if (Vector3.Distance(transform.position, _player.transform.position) < 4) // detect player distance
            {
                _aiService.AddToChase(gameObject);
            }

            if (_aiService.AllowedChasingPlayer(gameObject))
            {
                _patrolBehaviour.CancelPatrol();
                _followPlayerBehaviour.MoveToPlayer(_aiService.EnemiesCount < 10 ? 1 : -1);
            }
            else
            {
                _patrolBehaviour.PatrolArea();
            }
        }
        
        private void DecideAttackStrategy()
        {
            _reconsiderationTime -= Time.deltaTime;
            
            if(CanNotAttack())
                return;
            
            if(_followPlayerBehaviour.PlayerReached)
                UpdateAttackDelayCountdown();
            else
                ResetAttackDelayCountdown();

            
            if (ComboAttackConditionsFulfilled())
            {
                _yithRageComboAbility.PerformCombo();
                _reconsiderationTime = _yithConfiguration.ReconsiderationTime;
                return;
            }

            if (CanDoBasicAttack())
            {
                _yithAttack.PerformAttack();
                ResetAttackDelayCountdown();
                _reconsiderationTime = _yithConfiguration.ReconsiderationTime;
            }
        }

        private void UpdateAttackDelayCountdown() => 
            _attackDelayTime -= Time.deltaTime;

        private void ResetAttackDelayCountdown() => 
            _attackDelayTime = 1f;

        private bool AttackDelayCountdownIsUp() 
            => _attackDelayTime <= 0;

        private bool CanDoBasicAttack()
        {
            return _followPlayerBehaviour.PlayerReached
                   && _yithAttack.CanAttack() 
                   && AttackDelayCountdownIsUp();
        }

        private bool ComboAttackConditionsFulfilled()
            => CanDoComboAttack() && _rageComboRandom > 0.5f;
        
        private bool CanDoComboAttack()
        {
            if (_followPlayerBehaviour.DistanceToPlayer < _yithConfiguration.ComboAttackTriggerDistance.x
                || _followPlayerBehaviour.DistanceToPlayer > _yithConfiguration.ComboAttackTriggerDistance.y)
                return false;
            
            return _yithRageComboAbility.CanComboAttack();
        }

        private bool CanNotAttack()
        {
            return _yithRageComboAbility.InProcess || _reconsiderationTime > 0;
        }
    }
}