using System;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    public class TentacleAggro : MonoBehaviour
    {
        public bool HasAggro { get; private set; }
        public bool IsPlayerInFront { get; private set; }
        
        [SerializeField] private TentacleAnimator _tentacleAnimator;
        [SerializeField] private EnemyStatsContainer _enemyStatsContainer;
        
        private PlayerFacade _player;
        private TentacleConfiguration _tentacleConfiguration;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _player = gameFactory.Player;
        }

        private void Start()
        {
            Debug.Log(_enemyStatsContainer.Config);
            _tentacleConfiguration = (TentacleConfiguration)_enemyStatsContainer.Config;
        }

        private void Update()
        {
            if (PlayerInRange())
            {
                _tentacleAnimator.PlayAggroIdle();
                HasAggro = true;
                
                Vector3 directionToPlayer = _player.transform.position - transform.position;
                directionToPlayer.Normalize();

                if (DistanceToPlayer() < 3 && Vector3.Dot(transform.forward, directionToPlayer) > 0.7f)
                {
                    IsPlayerInFront = true;
                }
                else
                {
                    IsPlayerInFront = false;
                }
            }
            else
            {
                HasAggro = false;
                IsPlayerInFront = false;
            }
        }
        
        private bool PlayerInRange()
        {
            return DistanceToPlayer() < _tentacleConfiguration.AggroRange;
        }
        
        private float DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position);
        }
    }
}
