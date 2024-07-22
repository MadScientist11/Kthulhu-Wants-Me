using KthulhuWantsMe.Source.Gameplay.Enemies.AI;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
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
        [SerializeField] private CyaeghaMovement _cyaeghaMovement;

        [SerializeField] private Patrol _patrolBehaviour;
        [SerializeField] private FollowPlayer _followPlayerBehaviour;
        

        public bool BlockProcessing { get; set; }
        

        private float _attackDelayTime;
        private float _reconsiderationTime;

        private CyaeghaConfiguration _cyaeghaConfiguration;
        
        private PlayerFacade _player;
        private IAIService _aiService;

        [Inject]
        public void Construct(IPlayerProvider playerProvider, IAIService aiService)
        {
            _aiService = aiService;
            _player = playerProvider.Player;
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
            
            _reconsiderationTime -= Time.deltaTime;

            if (_reconsiderationTime > 0)
            {
                return;
            }
            
            DecideMoveStrategy();
            DecideAttackStrategy();
        }

        private void DecideMoveStrategy()
        {
            if(_cyaeghaAttack.IsAttacking)
                return;
            
            if (Vector3.Distance(transform.position, _player.transform.position) < 4)
            {
                _aiService.AddToChase(gameObject);
            }

            if (_aiService.AllowedChasingPlayer(gameObject))
            {
                _patrolBehaviour.CancelPatrol();
                _cyaeghaMovement.FollowPlayer();
            }
            else
            {
                _patrolBehaviour.PatrolArea();
            }
        }

        private void DecideAttackStrategy()
        {
           if (_followPlayerBehaviour.PlayerReached)
                UpdateAttackDelayCountdown();
           else
                ResetAttackDelayCountdown();


           if (CanDoBasicAttack())
           {
               _cyaeghaAttack.PerformAttack(_player.transform.position);
               _reconsiderationTime = 2f;
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
            return _aiService.CanAttack() &&
                    _cyaeghaAttack.CanAttack()
                   && AttackDelayCountdownIsUp();
        }
    }
}