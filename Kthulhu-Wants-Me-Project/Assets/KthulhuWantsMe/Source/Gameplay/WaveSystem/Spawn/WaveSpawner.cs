using System;
using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem.Spawn
{
    public class WaveSpawner
    {
        
        public event Action<IEnumerable<Health>> BatchSpawned;
        
        public IOrderedEnumerable<EnemySpawner> ClosestSpawners
        {
            get
            {
                return _enemySpawners
                    .Select(spawner => spawner.Value)
                    .OrderBy(spawner => Vector3.Distance(spawner.Position, _gameFactory.Player.transform.position));
            }
        }

        public EnemySpawner ClosestSpawner
        {
            get
            {
                return ClosestSpawners.First();
            }
        }


        private Dictionary<EnemySpawnerId, EnemySpawner> _enemySpawners;
        private WaveState _waveState;

        private readonly IGameFactory _gameFactory;
        private readonly ISceneDataProvider _sceneDataProvider;

        public WaveSpawner(IGameFactory gameFactory, ISceneDataProvider sceneDataProvider)
        {
            _sceneDataProvider = sceneDataProvider;
            _gameFactory = gameFactory;
        }

        public void Initialize(WaveState waveState)
        {
            _enemySpawners = _sceneDataProvider.AllSpawnPoints[SpawnPointType.EnemySpawner]
                .ToDictionary(sp => sp.EnemySpawnerId, CreateSpawnerFrom);

            EnemySpawner CreateSpawnerFrom(SpawnPoint sp)
            {
                EnemySpawner spawner = new EnemySpawner(_gameFactory, sp);
                return spawner;
            }

            _waveState = waveState;
        }

        public void SpawnBatchNotified(Batch batch)
        {
            IEnumerable<Health> spawnedBatch = SpawnBatch(batch);
            BatchSpawned?.Invoke(spawnedBatch);
        }
        
        public Health SpawnEnemyClosestToPlayer(EnemyType enemyType)
        {
            return SpawnEnemy(ClosestSpawner, enemyType);
        }

        public Health SpawnEnemy(EnemySpawner spawner, EnemyType enemyType)
        {
            Health enemyHealth = spawner.Spawn(enemyType);
            _waveState.RegisterEnemy(spawner.Id, enemyHealth);
            return enemyHealth;
        }

        private IEnumerable<Health> SpawnBatch(Batch batch)
        {
            List<Health> batchEnemies = new();
            foreach (EnemyPack enemyPack in batch.EnemyPack)
                batchEnemies.AddRange(SpawnEnemyPack(enemyPack));
            return batchEnemies;
        }

        private IEnumerable<Health> SpawnEnemyPack(EnemyPack enemyPack)
        {
            for (int i = 0; i < enemyPack.Quantity; i++)
            {
                EnemySpawner spawner = FindAppropriateSpawnerFor(enemyPack);
                var enemyHealth = SpawnEnemy(spawner, enemyPack.EnemyType);
                yield return enemyHealth;
            }
        }

        private EnemySpawner FindAppropriateSpawnerFor(EnemyPack batchEntry)
        {
            if (batchEntry.SpawnAt == EnemySpawnerId.Default &&
                (batchEntry.EnemyType == EnemyType.Tentacle || batchEntry.EnemyType == EnemyType.BleedTentacle ||
                 batchEntry.EnemyType == EnemyType.PoisonousTentacle))
                return FindUnoccupiedSpawner();
            else if (batchEntry.SpawnAt != EnemySpawnerId.Default)
                return _enemySpawners[batchEntry.SpawnAt];
            else
                return ClosestSpawner;
        }

        private EnemySpawner FindUnoccupiedSpawner()
        {
            foreach (EnemySpawner closestSpawner in ClosestSpawners)
            {
                if (!_waveState.AliveEnemiesByPlace.TryGetValue(closestSpawner.Id, out List<Health> enemies) ||
                    enemies == null)
                {
                    return closestSpawner;
                }

                if (enemies.All(enemy =>
                        enemy.GetComponent<EnemyStatsContainer>().Config.EnemyType != EnemyType.Tentacle))
                {
                    return closestSpawner;
                }
            }

            return null;
        }
    }
}