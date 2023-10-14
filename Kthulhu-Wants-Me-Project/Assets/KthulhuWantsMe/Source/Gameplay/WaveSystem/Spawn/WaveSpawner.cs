using System;
using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
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
        private readonly IDataProvider _dataProvider;

        public WaveSpawner(IGameFactory gameFactory, ISceneDataProvider sceneDataProvider, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _sceneDataProvider = sceneDataProvider;
            _gameFactory = gameFactory;
        }

        public void Initialize(WaveState waveState)
        {
            _enemySpawners = _sceneDataProvider.AllSpawnPoints[SpawnPointType.EnemySpawner]
                .ToDictionary(sp => sp.EnemySpawnerId, CreateSpawnerFrom);

            EnemySpawner CreateSpawnerFrom(SpawnPoint sp)
            {
                EnemySpawner spawner = new EnemySpawner(_gameFactory, _dataProvider, sp);
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

        public bool IsSpawnerVacant(EnemySpawnerId spawnerId)
        {
            return !_waveState.AliveEnemiesByPlace.ContainsKey(spawnerId) ||
                   (_waveState.AliveEnemiesByPlace[spawnerId]
                       .All(enemyHealth => !enemyHealth.GetComponent<EnemyStatsContainer>().Config.IsElite() 
                                           && !_waveState.PendingEnemies.ContainsKey(spawnerId)));
        }

        public void RespawnClosest(EnemySpawnerId currentSpawner, Health entity)
        {
            if (_waveState.PendingEnemies.ContainsValue(entity))
                return;

            EnemySpawner desired = ClosestSpawner;
            
            if(_waveState.PendingEnemies.ContainsKey(desired.Id))
                return;

            _waveState.DeregisterEnemy(currentSpawner, entity);
            _waveState.RegisterEnemyAsPending(desired.Id, entity);

            if (entity.TryGetComponent(out TentacleRetreat retreatBehaviour))
            {
                retreatBehaviour.Retreat(false, () =>
                {
                    desired.Spawn(entity, EnemyType.Tentacle, () =>
                    {
                        _waveState.DeregisterEnemyAsPending(desired.Id, entity);
                        _waveState.RegisterEnemy(desired.Id, entity);
                    });
                });
            }
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
                Health enemyHealth = SpawnEnemy(spawner, enemyPack.EnemyType);
                yield return enemyHealth;
            }
        }

        private EnemySpawner FindAppropriateSpawnerFor(EnemyPack batchEntry)
        {
            EnemyConfiguration enemyConfig = _dataProvider.EnemyConfigsProvider.EnemyConfigs[batchEntry.EnemyType];

            if (batchEntry.SpawnAt == EnemySpawnerId.Default && enemyConfig.IsElite())
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
                if(IsSpawnerVacant(closestSpawner.Id))
                {
                    return closestSpawner;
                }
            }

            return null;
        }
    }
}