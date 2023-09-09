using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaAIBrain : MonoBehaviour
    {
        [SerializeField] private CyaeghaHealth _cyaeghaHealth;
        [SerializeField] private CyaeghaAttack _cyaeghaAttack;
        [SerializeField] private FollowLogic _followLogic;

        private bool _isDead;
        private float _attackDelayTime;
        private float _rageComboRandom;

        private const float ComboAttackReavaluationTime = 5f;

        private PlayerFacade _player;
        private YithConfiguration _yithConfiguration;

        [Inject]
        public void Construct(IDataProvider dataProvider, IGameFactory gameFactory, IRandomService randomService)
        {
            _yithConfiguration = dataProvider.YithConfig;
            _player = gameFactory.Player;
            
            _cyaeghaHealth.Died += TriggerDeath;
            
            randomService.ProvideRandomValue(value => _rageComboRandom = value, ComboAttackReavaluationTime);
        }

        private void OnDestroy()
        {
            _cyaeghaHealth.Died -= TriggerDeath;
        }

        private void Start()
        {
            _followLogic.Init(_player.transform, Mathf.Infinity, 1.5f);
        }

        private void Update() => 
            DecideStrategy();

        public void ResetState()
        {
        }

        private void DecideStrategy()
        {
            if(_isDead)
                return;
            
            DecideMoveStrategy();
            DecideAttackStrategy();
        }

        private void DecideMoveStrategy()
        {
            if (ShouldFollow())
                _followLogic.Follow();
            else
                _followLogic.StopFollowing();
        }
        
        private void DecideAttackStrategy()
        {
      
            
            if(_followLogic.TargetReached)
                UpdateAttackDelayCountdown();
            else
                ResetAttackDelayCountdown();



            if (CanDoBasicAttack())
            {
                _cyaeghaAttack.PerformAttack();
                ResetAttackDelayCountdown();
            }
        }

        private void TriggerDeath()
        {
            _cyaeghaHealth.HandleDeath();
            _isDead = true;
        }

        private void UpdateAttackDelayCountdown() => 
            _attackDelayTime -= Time.deltaTime;

        private void ResetAttackDelayCountdown() => 
            _attackDelayTime = _yithConfiguration.AttackDelay;

        private bool AttackDelayCountdownIsUp() 
            => _attackDelayTime <= 0;

        private bool ShouldFollow() => 
            _followLogic.CanFollow() && !_followLogic.TargetReached;

        private bool CanDoBasicAttack()
        {
            return _followLogic.TargetReached
                   && _cyaeghaAttack.CanAttack() 
                   && AttackDelayCountdownIsUp();
        }


    }
}
