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
      
        [SerializeField] private YithHealth _yithHealth;
        [SerializeField] private YithAttack _yithAttack;
        [SerializeField] private YithRageComboAbility _yithRageComboAbility;
        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;

        [SerializeField] private FollowPlayer _followPlayerBehaviour;
        [SerializeField] private Patrol _patrolBehaviour;

        private float _attackDelayTime;
        private float _rageComboRandom;

        private YithConfiguration _yithConfiguration;

        private const float ComboAttackReavaluationTime = 5f;

        private PlayerFacade _player;

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

        private void Update() => 
            DecideStrategy();

        public void ResetState()
        {
        }

        private float _behaviourReavaluationTime = 5f;
        private float _behaviourReavaluationCooldown;
        private IAIService _aiService;

        private void DecideStrategy()
        {
            if(_yithHealth.IsDead || BlockProcessing)
                return;

            DecideMoveStrategy();
            DecideAttackStrategy();
        }

        private void DecideMoveStrategy()
        {
            if (_yithRageComboAbility.InProcess)
            {
                _followPlayerBehaviour.MoveToPlayer();
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
            if(_yithRageComboAbility.InProcess)
                return;
            
            if(_followPlayerBehaviour.PlayerReached)
                UpdateAttackDelayCountdown();
            else
                ResetAttackDelayCountdown();


            if (ComboAttackConditionsFulfilled())
            {
                _yithRageComboAbility.PerformCombo();
                return;
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
            return _followPlayerBehaviour.DistanceToPlayer is < 6f and > 4f && _yithRageComboAbility.CanComboAttack();
        }
    }
}