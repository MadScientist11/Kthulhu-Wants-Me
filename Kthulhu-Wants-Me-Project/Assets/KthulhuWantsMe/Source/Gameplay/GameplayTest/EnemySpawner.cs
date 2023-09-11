using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.GameplayTest
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyType _enemyType;
        
        private IGameFactory _gameFactory;
        private IPortalFactory _portalFactory;

        [Inject]
        public void Construct(IGameFactory gameFactory, IPortalFactory portalFactory)
        {
            _portalFactory = portalFactory;
            _gameFactory = gameFactory;
        }

        private void Start() => 
            SpawnEnemy();


        [Button()]
        private void SpawnNewEnemy()
        {
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            switch (_enemyType)
            {
                case EnemyType.Tentacle:
                    Portal portal1 = _portalFactory.GetOrCreatePortal(transform.position, Quaternion.identity,
                        PortalFactory.PortalType.TentaclePortal);
                    portal1.StartEnemySpawn();
                    break;
                case EnemyType.PoisonousTentacle:
                    Portal portal2 = _portalFactory.GetOrCreatePortal(transform.position, Quaternion.identity,
                        PortalFactory.PortalType.PoisonTentaclePortal);
                    portal2.StartEnemySpawn();
                    break;
                case EnemyType.Yith:
                    _gameFactory.CreateEnemy(transform.position, Quaternion.identity, EnemyType.Yith);
                    break;
                case EnemyType.Cyeagha:
                    _gameFactory.CreateEnemy(transform.position, Quaternion.identity, EnemyType.Cyeagha);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}