using Freya;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem.Spawn
{
    public class EnemySpawner
    {
        public Vector3 Position => _spawnPoint.Position;
        public EnemySpawnerId Id => _spawnPoint.EnemySpawnerId;

        private readonly IGameFactory _gameFactory;
        private readonly SpawnPoint _spawnPoint;

        public EnemySpawner(IGameFactory gameFactory, SpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _gameFactory = gameFactory;
        }


        public Health Spawn(EnemyType enemyType)
        {
            Vector3 randomPoint = Mathfs.Abs(Random.insideUnitSphere * 3f);
            Vector3 spawnPosition = _spawnPoint.Position + randomPoint;

            if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hitInfo, 100))
            {
                Portal portal =
                    _gameFactory.CreatePortalWithEnemy(hitInfo.point + Vector3.one * 0.05f, Quaternion.identity,
                        enemyType);
                GameObject enemy = portal.LastSpawnedEnemy;

                return enemy.GetComponent<Health>();
            }

            return null;
        }
    }
}