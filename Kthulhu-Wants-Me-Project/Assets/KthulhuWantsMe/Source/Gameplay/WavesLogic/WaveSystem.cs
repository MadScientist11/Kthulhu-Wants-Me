using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Freya;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

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


        private Dictionary<WaveObjective, IWaveScenario> _waveScenarios;
        private IWaveScenario _currentWaveScenario;

        private const int NextWaveAfterSeconds = 10;

        private Dictionary<EnemySpawnerId, Spawner> _enemySpawners;
        private Dictionary<EnemySpawnerId, List<Health>> _aliveEnemies = new();

        private readonly IProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly ISceneDataProvider _sceneDataProvider;
        private readonly Waves _wavesData;
        private GameStateMachine _gameStateMachine;


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
            OnWaveCompleted?.Invoke();
            _gameStateMachine.SwitchState<BetweenWavesState>();
        }


        public void StartNextWave()
        {
            SwitchState(WaveState.WaveStart);
            //CurrentWaveEnemies = SpawnWaveEnemies(waveEnemies);
            //_currentWaveScenario = _waveScenarios[waveData.WaveObjective];
            //_currentWaveScenario.Initialize();
        }

        private int _currentBatch;
        private int AliveEnemiesLeft => _aliveEnemies.Select(spawner => spawner.Value.Count).Sum();

        private void SwitchState(WaveState waveState)
        {
            switch (waveState)
            {
                case WaveState.WaveStart:
                    SwitchState(WaveState.SpawnBatch);
                    break;
                case WaveState.SpawnBatch:
                    ProcessBatch();
                    break;
                case WaveState.WaveEnd:
                    CompleteWave();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(waveState), waveState, null);
            }
        }

        private void ProcessBatch()
        {
            List<(EnemySpawnerId spawnedAt, Health enemyHealth)> batchEnemies = SpawnBatch().ToList();
            foreach (var enemy in batchEnemies)
            {
                _aliveEnemies.GetOrCreate(enemy.spawnedAt).Add(enemy.enemyHealth);
                
                enemy.enemyHealth.Died += 
                    () => TrackEnemiesDeath(enemy.spawnedAt, enemy.enemyHealth);
            }
            
            _currentBatch++;
            SpawnNextBatchIfAny().Forget();
        }

        private void TrackEnemiesDeath(EnemySpawnerId wasSpawnedAt, Health deathHealth)
        {
            int waveIndex = _progressService.ProgressData.WaveIndex;
            WaveData waveData = _wavesData[waveIndex];
            
            _aliveEnemies[wasSpawnedAt].Remove(deathHealth);

            
            if (AliveEnemiesLeft == 0 && waveData.Batches.Count <= _currentBatch)
                SwitchState(WaveState.WaveEnd);
        }
        private IEnumerable<(EnemySpawnerId spawnerId, Health)> SpawnBatch()
        {
            int waveIndex = _progressService.ProgressData.WaveIndex;
            WaveData waveData = _wavesData[waveIndex];
            Batch batch = waveData.Batches[_currentBatch];

            foreach (WaveEnemy waveEnemy in batch.WaveEnemies)
            {
                if (waveEnemy.SpawnAt == EnemySpawnerId.Default)
                {
                    foreach (Health enemyHealth in _enemySpawners[EnemySpawnerId.EnemySpawner1].Spawn(waveEnemy))
                        yield return (EnemySpawnerId.EnemySpawner1, enemyHealth);

                    // Spawn at nearest to player spawner
                    continue;
                }

                foreach (Health enemyHealth in _enemySpawners[waveEnemy.SpawnAt].Spawn(waveEnemy))
                    yield return (waveEnemy.SpawnAt, enemyHealth);
            }
        }

        private async UniTaskVoid SpawnNextBatchIfAny()
        {
            int waveIndex = _progressService.ProgressData.WaveIndex;
            WaveData waveData = _wavesData[waveIndex];

            await UniTask.Delay(5000);

            if(waveData.Batches.Count > _currentBatch)
                SwitchState(WaveState.SpawnBatch);
        }

        //private async void OnWaveCompleted()
        //{
        //    _currentWaveScenario.OnWaveCompleted -= OnWaveCompleted;
//
        //    _progressService.ProgressData.WaveIndex++;
        //    await UniTask.Delay(5000);
        //    StartNextWave();
        //}


        private EnemySpawnZoneData GetSpawnZoneFor(WaveEnemy waveEnemy)
        {
            if (waveEnemy.SpawnAt == EnemySpawnerId.Default)
            {
                if (waveEnemy.EnemyType == EnemyType.Tentacle)
                {
                }

                return FindNearPlayerSpawnZone();
            }


            EnemySpawnZoneData spawnZone = _sceneDataProvider.AllSpawnPoints[SpawnPointType.EnemySpawner]
                .Where(sp => sp.EnemySpawnerId == waveEnemy.SpawnAt)
                .Select(ToSpawnZone).First();

            if (spawnZone == null)
            {
                Debug.LogError($"Couldn't find EnemySpawner with the {waveEnemy.SpawnAt} id");
            }

            return spawnZone;
        }

        private EnemySpawnZoneData FindNearPlayerSpawnZone()
        {
            List<EnemySpawnZoneData> enemySpawnZones = _sceneDataProvider.AllSpawnPoints[SpawnPointType.EnemySpawner]
                .Select(ToSpawnZone).OrderBy(sp =>
                    Vector3.Distance(sp.Position, _gameFactory.Player.transform.position)).ToList();

            return enemySpawnZones[0];
        }

        private Spawner FindClosestSpawner()
        {
            return _enemySpawners
                    .Select(spawner => spawner.Value)
                    .OrderBy(spawner => Vector3.Distance(spawner.Position, _gameFactory.Player.transform.position))
                    .First();

        }

        private List<EnemyStatsContainer> SpawnWaveEnemies(List<WaveEnemy> waveEnemiesData)
        {
            List<EnemyStatsContainer> waveEnemies = new();
            foreach (WaveEnemy waveEnemy in waveEnemiesData)
            {
                for (int i = 0; i < waveEnemy.Quantity; i++)
                {
                    EnemySpawnZoneData spawnZone = GetSpawnZoneFor(waveEnemy);
                    Vector3 randomPoint = Mathfs.Abs(Random.insideUnitSphere * spawnZone.Radius);

                    Vector3 spawnPosition =
                        spawnZone.Position +
                        randomPoint;
                    if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hitInfo, 100))
                    {
                        Portal portal =
                            _gameFactory.CreatePortalWithEnemy(hitInfo.point + Vector3.one * 0.05f, Quaternion.identity,
                                waveEnemy.EnemyType);
                        GameObject enemy = portal.LastSpawnedEnemy;
                        waveEnemies.Add(enemy.GetComponent<EnemyStatsContainer>());
                    }
                }
            }

            return waveEnemies;
        }

        private EnemySpawnZoneData ToSpawnZone(SpawnPoint spawnPoint)
        {
            return new EnemySpawnZoneData()
            {
                Position = spawnPoint.Position,
                Radius = 5f,
            };
        }
    }

    public class Spawner
    {
        public Vector3 Position => _spawnPoint.Position;
        
        private readonly IGameFactory _gameFactory;
        private readonly SpawnPoint _spawnPoint;

        public Spawner(IGameFactory gameFactory, SpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _gameFactory = gameFactory;
        }


        public IEnumerable<Health> Spawn(WaveEnemy waveEnemy)
        {
            List<Health> enemies = new();
            for (int i = 0; i < waveEnemy.Quantity; i++)
            {
                Vector3 randomPoint = Mathfs.Abs(Random.insideUnitSphere * 3f);
                Vector3 spawnPosition = _spawnPoint.Position + randomPoint;

                if (Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hitInfo, 100))
                {
                    Portal portal =
                        _gameFactory.CreatePortalWithEnemy(hitInfo.point + Vector3.one * 0.05f, Quaternion.identity, waveEnemy.EnemyType);
                    GameObject enemy = portal.LastSpawnedEnemy;
                    enemies.Add(enemy.GetComponent<Health>());
                }
                
            }

            return enemies;
        }
    }
}