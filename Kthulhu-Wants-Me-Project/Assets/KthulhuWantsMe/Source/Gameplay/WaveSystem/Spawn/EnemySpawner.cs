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

        private Vector3 SpawnPointFor(EnemyConfiguration enemyConfig)
        {
            Vector3 randomOffset = Random.insideUnitCircle.XZtoXYZ() * Radius;
            
            Vector3 spawnPoint = _spawnPoint.Position.AddY(5) + randomOffset;
            
            if (enemyConfig.IsElite())
                spawnPoint = Position.AddY(5);
            
            return spawnPoint;
        }

        public Health Spawn(EnemyType enemyType)
        {
            EnemyConfiguration enemyConfig = _dataProvider.EnemyConfigsProvider.EnemyConfigs[enemyType];
            Vector3 spawnPosition = SpawnPointFor(enemyConfig);
            
            int iterations = 0;
            RaycastHit hitInfo;

            while (!Physics.Raycast(spawnPosition, Vector3.down, out hitInfo, 100, LayerMasks.GroundMask))
            {
                if (iterations > 100)
                {
                    Debug.LogError($"Couldn't spawn an enemy {enemyType} at {Id}, position was {spawnPosition} make sure floor has Ground layer.");
                    return null;
                }
                spawnPosition = SpawnPointFor(enemyConfig);
                iterations++;
            }


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