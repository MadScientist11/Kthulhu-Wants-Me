using KthulhuWantsMe.Source.Gameplay.Enemies.Yith;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    public class CyaeghaAIBrain : MonoBehaviour
    {
        [SerializeField] private CyaeghaHealth _cyaeghaHealth;
        [SerializeField] private CyaeghaAttack _cyaeghaAttack;
        [SerializeField] private FollowLogic _followLogic;
        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;

        private float _attackDelayTime;

        private PlayerFacade _player;
        private CyaeghaConfiguration _cyaeghaConfiguration;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }

        private void Start()
        {
            _followLogic.Init(_player.transform, Mathf.Infinity, 3f);
            _cyaeghaConfiguration = (CyaeghaConfiguration)_enemyStatsContainer.Config;
            _followLogic.FollowSpeed = _cyaeghaConfiguration.MoveSpeed;
        }

        private void Update() => 
            DecideStrategy();

        public void ResetState()
        {
        }

        private void DecideStrategy()
        {
            if(_cyaeghaHealth.IsDead)
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
