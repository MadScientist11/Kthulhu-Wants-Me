using System;
using System.Collections.Generic;
using System.Linq;
using Freya;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.WavesLogic
{
    public interface IWaveScenario
    {
        event Action OnWaveCompleted;
        void Initialize();
    }

    public class EliminateAllEnemiesScenario : IWaveScenario
    {
        public event Action OnWaveCompleted;

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
            foreach (Enemy waveEnemy in _waveSystem.CurrentWaveEnemies)
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
                OnWaveCompleted?.Invoke();
            }

            Debug.Log("Decrese amount");
        }
       
    }

    public interface IWaveSystem
    {
        void StartNextWave();
    }

    public class WaveSystem : IWaveSystem, IInitializable
    {
        public List<Enemy> CurrentWaveEnemies
        {
            get;
            private set;
        }
        
        private Waves _wavesData;
        private IGameFactory _gameFactory;
        private IProgressService _progressService;
        private Location _location;

        private Dictionary<WaveObjective, IWaveScenario> _waveScenarios;
        private IWaveScenario _currentWaveScenario;


        public WaveSystem(IDataProvider dataProvider, IGameFactory gameFactory, IProgressService progressService)
        {
            _location = dataProvider.Locations[LocationId.GameLocation];
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

        public void StartNextWave()
        {
            int waveIndex = _progressService.ProgressData.WaveIndex;
            WaveData waveData = _wavesData[waveIndex];
            List<WaveEnemy> waveEnemies = waveData.WaveEnemies;
            
            CurrentWaveEnemies = SpawnWaveEnemies(waveEnemies);
            _currentWaveScenario = _waveScenarios[waveData.WaveObjective];
            _currentWaveScenario.Initialize();
            _currentWaveScenario.OnWaveCompleted += OnWaveCompleted;
        }

        private void OnWaveCompleted()
        {
            _currentWaveScenario.OnWaveCompleted -= OnWaveCompleted;

            _progressService.ProgressData.WaveIndex++;
            StartNextWave();
        }

        
        private List<Enemy> SpawnWaveEnemies(List<WaveEnemy> waveEnemiesData)
        {
            List<Enemy> waveEnemies = new();
            foreach (WaveEnemy waveEnemy in waveEnemiesData)
            {
                for (int i = 0; i < waveEnemy.Quantity; i++)
                {
                    EnemySpawnZoneData spawnZone =
                        FindNearPlayerSpawnZone();
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
                        waveEnemies.Add(enemy.GetComponent<Enemy>());
                    }
                }
            }

            return waveEnemies;
        }

        private EnemySpawnZoneData FindNearPlayerSpawnZone()
        {
            List<EnemySpawnZoneData> enemySpawnZones = _location.EnemySpawnZones.OrderBy(sp =>
                Vector3.Distance(sp.Position, _gameFactory.Player.transform.position)).ToList();

            return enemySpawnZones[0];
        }
    }
}