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

        [SerializeField] private YithMovement _yithMovement;
        [SerializeField] private FollowPlayer _followPlayerBehaviour;

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
                _yithMovement.Stop();
                return;
            }
            
            if (Vector3.Distance(transform.position, _player.transform.position) < _yithConfiguration.ChaseDistance) // detect player distance
            {
                _aiService.AddToChase(gameObject);
            }

            if (_aiService.AllowedChasingPlayer(gameObject))
            {
                _yithMovement.FollowPlayer();
            }
            else
            {
                _yithMovement.PatrolArea();
            }
        }
        
        private void DecideAttackStrategy()
        {
            _reconsiderationTime -= Time.deltaTime;
            
            if(CanNotAttack())
                return;
            
            if (ComboAttackConditionsFulfilled())
            {
                _yithRageComboAbility.PerformCombo();
                _reconsiderationTime = _yithConfiguration.ReconsiderationTime;
                return;
            }

            if (CanDoBasicAttack())
            {
                _yithAttack.PerformAttack().Forget();
                _reconsiderationTime = _yithConfiguration.ReconsiderationTime;
            }
        }

        private bool CanDoBasicAttack()
        {
            return _followPlayerBehaviour.PlayerReached
                   && _yithAttack.CanAttack();
        }

        private bool ComboAttackConditionsFulfilled()
            => CanDoComboAttack();
        
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