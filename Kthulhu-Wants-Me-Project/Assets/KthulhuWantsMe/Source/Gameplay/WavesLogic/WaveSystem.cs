using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Infrastructure;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.UI.Compass;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.WavesLogic
{
    public interface IWaveScenario
    {
        void Initialize(WaveData currentWave);
        void Dispose();
        void StartWaveScenario();
    }

    public class KillTentaclesSpecialScenario : IWaveScenario
    {
        private readonly WaveSystem _waveSystem;
        private readonly IUIService _uiService;
        private readonly IGameFactory _gameFactory;
        private WaveData _currentWave;
        private CompassUI _compassUI;
        private CancellationTokenSource _timerToken;


        public KillTentaclesSpecialScenario(WaveSystem waveSystem, IUIService uiService, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _uiService = uiService;
            _waveSystem = waveSystem;
        }

        public void Initialize(WaveData currentWave)
        {
            _currentWave = currentWave;
            _waveSystem.OnWaveCompleted += OnScenarioCompleted;
        }

        public void Dispose()
        {
            _waveSystem.OnWaveCompleted -= OnScenarioCompleted;
        }

        public void StartWaveScenario()
        {
            _waveSystem.ProcessBatch();
            ShowEnemiesOnCompass();
            StartTimer();
        }

        private void OnScenarioCompleted()
        {
            _compassUI.Hide();
            _timerToken.Cancel();
        }

        private void StartTimer()
        {
            _timerToken = new CancellationTokenSource();
            StartWaveTimer(_timerToken, 300).Forget();
        }

        private void ShowEnemiesOnCompass()
        {
            _compassUI = _uiService.MiscUIContainer.GetCompassUI();
            _compassUI.Show();
            _compassUI.Init(_gameFactory.Player.transform);
            
            foreach (Health aliveEnemy in _waveSystem.AliveEnemies)
            {
                Marker marker = new Marker()
                {
                    TrackedObject = aliveEnemy.transform
                };
                _compassUI.AddMarker(marker);

                aliveEnemy.Died += () => { _compassUI.RemoveMarker(marker);};
            }
        }

        private async UniTask StartWaveTimer(CancellationTokenSource cancellationToken, int timeInSeconds)
        {
            int countdown = timeInSeconds;
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay(1000);
                _uiService.MiscUIContainer.UpdateWaveCountdownText(countdown);
                countdown--;
                if (countdown == 0)
                {
                    cancellationToken.Cancel();
                    _compassUI.Hide();
                }
            }
        }
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

        public void Initialize(WaveData currentWave)
        {
          
        }

        public void Dispose()
        {
            
        }

        public void StartWaveScenario()
        {
            _waveSystem.ProcessBatch();
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
        public List<EnemyStatsContainer> CurrentWaveEnemies { get; private set; }

        public IWaveScenario CurrentScenario => _currentWaveScenario;
        public IEnumerable<Health> AliveEnemies => _aliveEnemies.Values.SelectMany(enemyList => enemyList);
        
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
        private readonly IUIService _uiService;


        public WaveSystem(ISceneDataProvider sceneDataProvider, IDataProvider dataProvider, IGameFactory gameFactory,
            IProgressService progressService,
            IUIService uiService,
            GameStateMachine gameStateMachine)
        {
            _uiService = uiService;
            _gameStateMachine = gameStateMachine;
            _sceneDataProvider = sceneDataProvider;
            _progressService = progressService;
            _gameFactory = gameFactory;
            _wavesData = dataProvider.Waves;
        }

        public void Initialize()
        {
            IWaveScenario eliminateAllEnemiesScenario = new EliminateAllEnemiesScenario(this, _wavesData);
            IWaveScenario killTentaclesSpecial = new KillTentaclesSpecialScenario(this, _uiService, _gameFactory);
            _waveScenarios = new()
            {
                { WaveObjective.KillAllEnemies, eliminateAllEnemiesScenario },
                { WaveObjective.KillTentaclesSpecial, killTentaclesSpecial }
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

        public void StartNextWave()
        {
            int waveIndex = _progressService.ProgressData.WaveIndex;
            WaveData waveData = _wavesData[waveIndex];
            _currentWaveScenario = _waveScenarios[waveData.WaveObjective];
            _currentWaveScenario.Initialize(waveData);
            _currentWaveScenario.StartWaveScenario();
        }

        public void CompleteWave()
        {
            _currentBatch = 0;
            _progressService.ProgressData.WaveIndex++;
            _batchSpawnCancellationToken.Cancel();
            OnWaveCompleted?.Invoke();
            _currentWaveScenario.Dispose();
            _gameStateMachine.SwitchState<BetweenWavesState>();
        }

        public void ProcessBatch()
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


            if (AliveEnemiesLeft == 0 && waveData.Batches.Count -1 <= _currentBatch)
            {
                CompleteWave();
                
            }
        }

        private async UniTaskVoid SpawnNextBatchIfAny(CancellationToken batchSpawnCancellationToken)
        {
            int waveIndex = _progressService.ProgressData.WaveIndex;
            WaveData waveData = _wavesData[waveIndex];
            
            if (waveData.Batches.Count - 1 <= _currentBatch)
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