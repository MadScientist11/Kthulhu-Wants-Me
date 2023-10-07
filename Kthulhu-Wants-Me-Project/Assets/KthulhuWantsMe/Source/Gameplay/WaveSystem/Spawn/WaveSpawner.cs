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
            get { return ClosestSpawners.First(); }
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

        public IEnumerable<Health> SpawnBatch(Batch batch)
        {
            List<Health> batchEnemies = new();
            foreach (EnemyPack enemyPack in batch.WaveEnemies)
                batchEnemies.AddRange(SpawnEnemyPack(enemyPack));
            return batchEnemies;
        }

        private IEnumerable<Health> SpawnEnemyPack(EnemyPack enemyPack)
        {
            for (int i = 0; i < enemyPack.Quantity; i++)
            {
                EnemySpawner spawner = FindAppropriateSpawnerFor(enemyPack);
                Health enemyHealth = spawner.Spawn(enemyPack.EnemyType);
                _waveState.RegisterEnemy(spawner.Id, enemyHealth);
                yield return enemyHealth;
            }
        }

        private EnemySpawner FindAppropriateSpawnerFor(EnemyPack batchEntry)
        {
            if (batchEntry.SpawnAt == EnemySpawnerId.Default &&
                (batchEntry.EnemyType == EnemyType.Tentacle || batchEntry.EnemyType == EnemyType.BleedTentacle ||
                 batchEntry.EnemyType == EnemyType.PoisonousTentacle))
                return FindUnoccupiedSpawner();
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