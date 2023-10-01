using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.WavesLogic
{
    public interface IWaveScenario
    {
        void Initialize();
    }

    public class EliminateAllEnemiesScenario : IWaveScenario
    {
        private int EnemiesCount => _waveSystem.CurrentWaveEnemies.Count;
        private int _currentEnemyCount;

        private Waves _wavesData;
        private WaveSystem _waveSystem;


        public EliminateAllEnemiesScenario(WaveSystem waveSystem, Waves wavesData)
        {
            _waveSystem = waveSystem;
            _wavesData = wavesData;
        }

        public void Initialize()
        {
            foreach (EnemyStatsContainer waveEnemy in _waveSystem.CurrentWaveEnemies)
            {
                // waveEnemy.GetComponent<Health>().Died += DecreaseEnemyCount;
            }

            _currentEnemyCount = EnemiesCount;
        }

        private void DecreaseEnemyCount()
        {
            _currentEnemyCount--;

            if (_currentEnemyCount == 0)
            {
                //_waveSystem.CompleteWave().Forget();
            }

            Debug.Log("Decrese amount");
        }
    }

    public interface IWaveSystem
    {
        event Action OnWaveCompleted;
        void StartNextWave();
        IWaveScenario CurrentScenario { get; }
    }

    public enum WaveState
    {
        WaveStart = 0,
        SpawnBatch = 1,
        WaveEnd = 2,
    }


    public class WaveSystem : IWaveSystem, IInitializable
    {
        public event Action OnWaveCompleted;
        public List<EnemyStatsContainer> CurrentWaveEnemies { get; private set; }

        public IWaveScenario CurrentScenario => _currentWaveScenario;
        
        private int AliveEnemiesLeft => _aliveEnemies.Select(spawner => spawner.Value.Count).Sum();


        private Dictionary<WaveObjective, IWaveScenario> _waveScenarios;
        private IWaveScenario _currentWaveScenario;

       
        private int _currentBatch;
        private CancellationTokenSource _batchSpawnCancellationToken;

        private Dictionary<EnemySpawnerId, Spawner> _enemySpawners;
        private readonly Dictionary<EnemySpawnerId, List<Health>> _aliveEnemies = new();

        private readonly IProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly ISceneDataProvider _sceneDataProvider;
        private readonly Waves _wavesData;
        private readonly GameStateMachine _gameStateMachine;


        public WaveSystem(ISceneDataProvider sceneDataProvider, IDataProvider dataProvider, IGameFactory gameFactory,
            IProgressService progressService,
            GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _sceneDataProvider = sceneDataProvider;
            _progressService = progressService;
            _gameFactory = gameFactory;
            _wavesData = dataProvider.Waves;
        }

        public void Initialize()
        {
            IWaveScenario eliminateAllEnemiesScenario = new EliminateAllEnemiesScenario(this, _wavesData);
            _waveScenarios = new()
            {
                { WaveObjective.KillAllEnemies, eliminateAllEnemiesScenario }
            };


            InitSpawners();
        }

        private void InitSpawners()
        {
            _enemySpawners = _sceneDataProvider.AllSpawnPoints[SpawnPointType.EnemySpawner]
                .ToDictionary(sp => sp.EnemySpawnerId, CreateSpawnerFrom);

            Spawner CreateSpawnerFrom(SpawnPoint sp)
            {
                Spawner spawner = new Spawner(_gameFactory, sp);
                return spawner;
            }
        }

        public void CompleteWave()
        {
            _currentBatch = 0;
            _progressService.ProgressData.WaveIndex++;
            _batchSpawnCancellationToken.Cancel();
            OnWaveCompleted?.Invoke();
            _gameStateMachine.SwitchState<BetweenWavesState>();
        }

        public void StartNextWave() => 
            ProcessBatch();

        private void ProcessBatch()
        {
            int waveIndex = _progressService.ProgressData.WaveIndex;
            WaveData waveData = _wavesData[waveIndex];
            Batch batch = waveData.Batches[_currentBatch];
            SpawnBatch(batch);
            
            _batchSpawnCancellationToken = new();
            SpawnNextBatchIfAny(_batchSpawnCancellationToken.Token).Forget();
        }

        private void SpawnBatch(Batch batch)
        {
            foreach (WaveEnemy batchEntry in batch.WaveEnemies) 
                SpawnWaveEnemy(batchEntry);
        }

        private void SpawnWaveEnemy(WaveEnemy batchEntry)
        {
            SingleEnemy singleEnemy = new SingleEnemy(batchEntry);
            for (int i = 0; i < batchEntry.Quantity; i++)
            {
                Spawner spawner = FindAppropriateSpawnerFor(batchEntry);
                Health enemyHealth = spawner.Spawn(singleEnemy);
                enemyHealth.Died +=
                    () => TrackEnemiesDeath(spawner.Id, enemyHealth);
                _aliveEnemies.GetOrCreate(spawner.Id).Add(enemyHealth);
            }
        }

        private void TrackEnemiesDeath(EnemySpawnerId wasSpawnedAt, Health deathHealth)
        {
            int waveIndex = _progressService.ProgressData.WaveIndex;
            WaveData waveData = _wavesData[waveIndex];

            _aliveEnemies[wasSpawnedAt].Remove(deathHealth);


            if (AliveEnemiesLeft == 0 && waveData.Batches.Count <= _currentBatch)
                CompleteWave();
        }

        private async UniTaskVoid SpawnNextBatchIfAny(CancellationToken batchSpawnCancellationToken)
        {
            int waveIndex = _progressService.ProgressData.WaveIndex;
            WaveData waveData = _wavesData[waveIndex];
            
            if (waveData.Batches.Count <= _currentBatch)
                return;

            int batchDelay = (int)waveData.Batches[_currentBatch].NextBatchDelay;
            await UniTask.Delay(TimeSpan.FromSeconds(batchDelay),false, PlayerLoopTiming.Update, batchSpawnCancellationToken);

            _currentBatch++;
            ProcessBatch();
        }

        private Spawner FindAppropriateSpawnerFor(WaveEnemy batchEntry)
        {
            if (batchEntry.SpawnAt == EnemySpawnerId.Default &&
                (batchEntry.EnemyType == EnemyType.Tentacle || batchEntry.EnemyType == EnemyType.BleedTentacle || batchEntry.EnemyType == EnemyType.PoisonousTentacle))
                return FindUnoccupiedSpawner();
            else
                return FindClosestSpawner();
        }

        private Spawner FindUnoccupiedSpawner()
        {
            foreach (Spawner closestSpawner in GetSortedClosestSpawners())
            {
                if (!_aliveEnemies.TryGetValue(closestSpawner.Id, out List<Health> enemies) || enemies == null)
                {
                    return closestSpawner;
                }

                if (enemies.All(enemy => enemy.GetComponent<EnemyStatsContainer>().Config.EnemyType != EnemyType.Tentacle))
                {
                    return closestSpawner;
                }
            }
            return null;
        }

        private Spawner FindClosestSpawner()
        {
            return GetSortedClosestSpawners()
                .First();
        }

        private IOrderedEnumerable<Spawner> GetSortedClosestSpawners()
        {
            return _enemySpawners
                .Select(spawner => spawner.Value)
                .OrderBy(spawner => Vector3.Distance(spawner.Position, _gameFactory.Player.transform.position));
        }
    }

    public class SingleEnemy
    {
        public EnemyType EnemyType => _waveEnemy.EnemyType;
        private readonly WaveEnemy _waveEnemy;

        public SingleEnemy(WaveEnemy waveEnemy)
        {
            _waveEnemy = waveEnemy;
        }
        
    }
}