using System;
using Freya;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem.Spawn
{
    public class EnemySpawner
    {
        public Vector3 Position => _spawnPoint.Position;
        public float Radius => _spawnPoint.Radius;
        public EnemySpawnerId Id => _spawnPoint.EnemySpawnerId;

        private readonly IGameFactory _gameFactory;
        private readonly SpawnPoint _spawnPoint;
        private readonly IDataProvider _dataProvider;

        public EnemySpawner(IGameFactory gameFactory, IDataProvider dataProvider, SpawnPoint spawnPoint)
        {
            _dataProvider = dataProvider;
            _spawnPoint = spawnPoint;
            _gameFactory = gameFactory;
        }


        public Health Spawn(EnemyType enemyType)
        {
            Vector3 randomPoint = Random.insideUnitCircle.XZtoXYZ() * Radius;
            Vector3 spawnPosition = _spawnPoint.Position.AddY(5) + randomPoint;
            
            EnemyConfiguration enemyConfig = _dataProvider.EnemyConfigsProvider.EnemyConfigs[enemyType];
            if (enemyConfig.IsElite())
                spawnPosition = Position.AddY(5);

            if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hitInfo, 100, LayerMasks.GroundMask))
            {
                GameObject enemy =
                    _gameFactory.CreatePortalWithEnemy(hitInfo.point + Vector3.one * 0.05f, Quaternion.identity,
                        enemyType);
                
                if (enemy.TryGetComponent(out ISpawnBehaviour spawnBehaviour))
                {
                    spawnBehaviour.OnSpawn();
                    spawnBehaviour.SpawnedAt = Id;
                }

                return enemy.GetComponent<Health>();
            }

            Debug.LogError("Couldn't spawn an enemy");
            return null;
        }
        
        public Health Spawn(Health enemyHealth, EnemyType enemyType, Action onSpawned)
        {
            Vector3 randomPoint = Random.insideUnitCircle.XZtoXYZ() * Radius;
            Vector3 spawnPosition = _spawnPoint.Position.AddY(5) + randomPoint;

            if (enemyType == EnemyType.Tentacle || enemyType == EnemyType.BleedTentacle ||
                enemyType == EnemyType.TentacleSpecial || enemyType == EnemyType.PoisonousTentacle)
                spawnPosition = Position.AddY(5);

            if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hitInfo, 100, LayerMasks.GroundMask))
            {
                enemyHealth.transform.position = hitInfo.point + Vector3.one * 0.05f;
                
                if (enemyHealth.TryGetComponent(out ISpawnBehaviour spawnBehaviour))
                {
                    spawnBehaviour.SpawnedAt = Id;
                    spawnBehaviour.OnSpawn(onSpawned);
                }

                return enemyHealth;
            }

            Debug.LogError("Couldn't spawn an enemy");
            return null;
        }
    }
}