using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public interface IEnemyAIBrain
    {
        bool BlockProcessing { get; }
    }

    public class CyaeghaAIBrain : MonoBehaviour, IEnemyAIBrain
    {
        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;
        [SerializeField] private CyaeghaHealth _cyaeghaHealth;
        [SerializeField] private CyaeghaAttack _cyaeghaAttack;

        [SerializeField] private Patrol _patrolBehaviour;
        [SerializeField] private FollowPlayer _followPlayerBehaviour;
        

        public bool BlockProcessing { get; set; }
        

        private float _attackDelayTime;

        private CyaeghaConfiguration _cyaeghaConfiguration;
        
        private PlayerFacade _player;
        private IAIService _aiService;

        [Inject]
        public void Construct(IGameFactory gameFactory, IAIService aiService)
        {
            _aiService = aiService;
            _player = gameFactory.Player;
        }

        private void Start()
        {
            _cyaeghaConfiguration = (CyaeghaConfiguration)_enemyStatsContainer.Config;
            _cyaeghaHealth.Revive();
        }

        private void Update() =>
            DecideStrategy();

        
        public void ResetAI()
        {
        }

        private void DecideStrategy()
        {
            if (_cyaeghaHealth.IsDead || BlockProcessing)
                return;

            DecideMoveStrategy();
            DecideAttackStrategy();
        }

        private void DecideMoveStrategy()
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < 4)
            {
                _aiService.AddToChase(gameObject);
            }

            if (_aiService.AllowedChasingPlayer(gameObject))
            {
                _patrolBehaviour.CancelPatrol();
                _followPlayerBehaviour.MoveToPlayer();
            }
            else
            {
                _patrolBehaviour.PatrolArea();
            }

        }

        private void DecideAttackStrategy()
        {
           //if (_followLogic.TargetReached)
                UpdateAttackDelayCountdown();
           // else
                ResetAttackDelayCountdown();


            if (CanDoBasicAttack())
            {
                _cyaeghaAttack.PerformAttack(_player.transform.position);
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
            return //_followLogic.TargetReached
                    _cyaeghaAttack.CanAttack()
                   && AttackDelayCountdownIsUp();
        }
    }
}