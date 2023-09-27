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
                waveEnemy.GetComponent<Health>().Died += DecreaseEnemyCount;
            }

            _currentEnemyCount = EnemiesCount;
        }

        private void DecreaseEnemyCount()
        {
            _currentEnemyCount--;

            if (_currentEnemyCount == 0)
            {
                _waveSystem.CompleteWave().Forget();
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

    public class WaveSystem : IWaveSystem, IInitializable
    {
        public event Action OnWaveCompleted;
        public List<EnemyStatsContainer> CurrentWaveEnemies
        {
            get;
            private set;
        }

        public IWaveScenario CurrentScenario => _currentWaveScenario;
     
        
        private Dictionary<WaveObjective, IWaveScenario> _waveScenarios;
        private IWaveScenario _currentWaveScenario;

        private const int NextWaveAfterSeconds = 10;
        
        
        private readonly IProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly ISceneDataProvider _sceneDataProvider;
        private readonly Waves _wavesData;
        private GameStateMachine _gameStateMachine;


        public WaveSystem(ISceneDataProvider sceneDataProvider, IDataProvider dataProvider, IGameFactory gameFactory, IProgressService progressService,
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
            _waveScenarios  = new() {
                { WaveObjective.KillAllEnemies, eliminateAllEnemiesScenario }
            };
        }

        public async UniTask CompleteWave()
        {
            OnWaveCompleted?.Invoke();

            _gameStateMachine.SwitchState<BetweenWavesState>();
        }

     
        public void StartNextWave()
        {
            int waveIndex = _progressService.ProgressData.WaveIndex;
            WaveData waveData = _wavesData[waveIndex];
            List<WaveEnemy> waveEnemies = waveData.WaveEnemies;
            
            CurrentWaveEnemies = SpawnWaveEnemies(waveEnemies);
            _currentWaveScenario = _waveScenarios[waveData.WaveObjective];
            _currentWaveScenario.Initialize();
        }

        //private async void OnWaveCompleted()
        //{
        //    _currentWaveScenario.OnWaveCompleted -= OnWaveCompleted;
//
        //    _progressService.ProgressData.WaveIndex++;
        //    await UniTask.Delay(5000);
        //    StartNextWave();
        //}

        
        private List<EnemyStatsContainer> SpawnWaveEnemies(List<WaveEnemy> waveEnemiesData)
        {
            List<EnemyStatsContainer> waveEnemies = new();
            foreach (WaveEnemy waveEnemy in waveEnemiesData)
            {
                for (int i = 0; i < waveEnemy.Quantity; i++)
                {
                    EnemySpawnZoneData spawnZone = GetSpawnZone(waveEnemy.SpawnAt);
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

        private EnemySpawnZoneData GetSpawnZone(EnemySpawnerId enemySpawnerId)
        {

            if (enemySpawnerId == EnemySpawnerId.Default)
                return FindNearPlayerSpawnZone();
            
            
            EnemySpawnZoneData spawnZone = _sceneDataProvider.AllSpawnPoints[SpawnPointType.EnemySpawner]
                .Where(sp => sp.EnemySpawnerId == enemySpawnerId)
                .Select(ToSpawnZone).First();

            if (spawnZone == null)
            {
                Debug.LogError($"Couldn't find EnemySpawner with the {enemySpawnerId} id");
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

        private EnemySpawnZoneData ToSpawnZone(SpawnPoint spawnPoint)
        {
            return new EnemySpawnZoneData()
            {
                Position = spawnPoint.Position,
                Radius = 5f,
            };
        }
    }
}