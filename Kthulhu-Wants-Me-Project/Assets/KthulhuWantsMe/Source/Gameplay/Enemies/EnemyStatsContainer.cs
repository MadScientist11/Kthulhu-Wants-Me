using System;
using System.Collections;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Enemies
{
    public class EnemyStatsContainer : MonoBehaviour
    {
        public EnemyStats EnemyStats { get; private set; }

        public EnemyType EnemyType { get; private set; }
        public EnemyConfiguration Config { get; private set; }
        
        private IDataProvider _dataProvider;
        private PlayerFacade _player;

        private NavMeshPath _navMeshPath;
        private NavMeshAgent _navMeshAgent;

        [Inject]
        public void Construct(IDataProvider dataProvider, IGameFactory gameFactory)
        {
            _dataProvider = dataProvider;
            _player = gameFactory.Player;
        }

        private void Start()
        {
            _navMeshPath = new NavMeshPath();
            TryGetComponent(out _navMeshAgent);
            StartCoroutine(CheckIsPlayerReachable());
        }

        public void Initialize(EnemyType enemyType, EnemyStats enemyStats)
        {
            EnemyStats = enemyStats;
            EnemyType = enemyType;
            Config = _dataProvider.EnemyConfigsProvider.EnemyConfigs[enemyType];
            Debug.Log($"Enemy {gameObject.name} initialized with {EnemyStats.Stats[StatType.MaxHealth]} hp");
        }


        private IEnumerator CheckIsPlayerReachable()
        {
            while (true)
            {
                yield return Utilities.WaitForSeconds.Wait(10);

                if (_navMeshAgent == null)
                {
                    yield break;
                }
                
                if(_navMeshAgent.enabled == false)
                    continue;

                if (_navMeshAgent.CalculatePath(_player.transform.position, _navMeshPath) && 
                    _navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                
                }
                else
                {
                    GetComponent<Health>().TakeDamage(100000);
                }
            }
        }
    }
}