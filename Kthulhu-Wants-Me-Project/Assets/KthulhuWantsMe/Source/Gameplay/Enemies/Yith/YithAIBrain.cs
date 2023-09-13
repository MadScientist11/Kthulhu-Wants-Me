using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    public class YithAIBrain : MonoBehaviour
    {
        public bool BlockProcessing { get; private set; }
        
        [SerializeField] private YithHealth _yithHealth;
        [SerializeField] private YithAttack _yithAttack;
        [SerializeField] private YithRageComboAbility _yithRageComboAbility;
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
            
            _yithHealth.Died += TriggerDeath;
            
            randomService.ProvideRandomValue(value => _rageComboRandom = value, ComboAttackReavaluationTime);
        }

        private void OnDestroy()
        {
            _yithHealth.Died -= TriggerDeath;
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
            if(_isDead || BlockProcessing)
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
                Debug.Log("Combo");
            }

            if (CanDoBasicAttack())
            {
                _yithAttack.PerformAttack();
                ResetAttackDelayCountdown();
            }
        }

        private void TriggerDeath()
        {
            _yithHealth.HandleDeath();
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
                   && _yithAttack.CanAttack() 
                   && AttackDelayCountdownIsUp();
        }

        private bool ComboAttackConditionsFulfilled()
            => CanDoComboAttack() && _rageComboRandom > 0.5f;
        
        private bool CanDoComboAttack()
        {
            return _followLogic.DistanceToTarget is < 6f and > 4f && _yithRageComboAbility.CanComboAttack();
        }
    }
}