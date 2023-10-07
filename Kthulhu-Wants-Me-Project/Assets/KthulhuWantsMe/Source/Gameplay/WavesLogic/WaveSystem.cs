using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
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

        private int _remainingTentacles;

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
            _waveSystem.OnBatchSpawned += ProcessSpecialTentacles;
        }

        public void Dispose()
        {
            _waveSystem.OnWaveCompleted -= OnScenarioCompleted;
            _waveSystem.OnBatchSpawned -= ProcessSpecialTentacles;
        }

        public void StartWaveScenario()
        {
            //_waveSystem.ProcessBatch();
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
            StartWaveTimer(_timerToken, _currentWave.TimeConstraint).Forget();
        }

        private void ShowEnemiesOnCompass()
        {
            _compassUI = _uiService.MiscUI.GetCompassUI();
            _compassUI.Show();
            _compassUI.Init(_gameFactory.Player.transform);

            foreach (Health aliveEnemy in _waveSystem.AliveEnemies)
            {
                Marker marker = new Marker()
                {
                    TrackedObject = aliveEnemy.transform
                };
                _compassUI.AddMarker(marker);

                aliveEnemy.Died += () => { _compassUI.RemoveMarker(marker); };
            }
        }

        private void ProcessSpecialTentacles(IEnumerable<Health> batchEnemies)
        {
            foreach (Health aliveEnemy in batchEnemies)
            {
                if (aliveEnemy.TryGetComponent(out TentacleAIBrain tentacleAIBrain))
                {
                    _remainingTentacles++;

                    aliveEnemy.Died += () =>
                    {
                        _remainingTentacles--;
                        if (_remainingTentacles == 0)
                        {
                            _waveSystem.KillAllAliveEnemies();
                        }
                    };
                }
            }
        }

        private async UniTask StartWaveTimer(CancellationTokenSource cancellationToken, int timeInSeconds)
        {
            int countdown = timeInSeconds;
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay(1000);
                _uiService.MiscUI.UpdateWaveCountdownText(countdown);
                countdown--;
                if (countdown == 0)
                {
                    cancellationToken.Cancel();
                    _compassUI.Hide();
                    List<Health> waveSystemAliveEnemies = _waveSystem.AliveEnemies.ToList();
                    foreach (Health aliveEnemy in waveSystemAliveEnemies)
                    {
                        if (aliveEnemy.TryGetComponent(out RetreatState retreatState))
                        {
                            retreatState.Retreat();
                        }
                    }

                    await UniTask.Delay(2000);
                    _waveSystem.CompleteWave();
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
            //_waveSystem.ProcessBatch();
        }
    }

    public interface IWaveSystem
    {
        event Action OnWaveCompleted;
        void StartWave(int waveIndex);
        IWaveScenario CurrentScenario { get; }
    }

    public class WaveState
    {
        public event Action BatchCleared;
        public event Action WaveCleared;

        public WaveObjective WaveObjective
        {
            get { return _waveData.WaveObjective; }
        }

        public Batch CurrentBatchData
        {
            get { return _waveData.Batches[_currentBatchIndex]; }
        }

        public int CurrentBatchIndex
        {
            get
            {
                return _currentBatchIndex;
            }
        }

        public Dictionary<EnemySpawnerId, List<Health>> AliveEnemiesByPlace
        {
            get { return _aliveEnemiesByPlace; }
        }


        private readonly Dictionary<EnemySpawnerId, List<Health>> _aliveEnemiesByPlace = new();
        private readonly List<Health> _aliveEnemies = new();

        private readonly WaveData _waveData;
        private int _currentBatchIndex;

        public WaveState(WaveData waveData)
        {
            _waveData = waveData;
        }

        public void RegisterEnemy(EnemySpawnerId enemySpawnPlace, Health enemy)
        {
            _aliveEnemiesByPlace.GetOrCreate(enemySpawnPlace).Add(enemy);
            _aliveEnemies.Add(enemy);

            enemy.Died += () => TrackEnemiesDeath(enemySpawnPlace, enemy);
        }

        public void ModifyBatchIndex(int index)
        {
            _currentBatchIndex = index;
        }

        private void TrackEnemiesDeath(EnemySpawnerId wasSpawnedAt, Health deathHealth)
        {
            _aliveEnemiesByPlace[wasSpawnedAt].Remove(deathHealth);
            _aliveEnemies.Remove(deathHealth);

            if (NoEnemiesLeft())
            {
                if (IsLastBatch())
                {
                    BatchCleared?.Invoke();
                    WaveCleared?.Invoke();
                    CleanUp();
                    return;
                }

                BatchCleared?.Invoke();
                _currentBatchIndex++;
            }
        }

        private void CleanUp()
        {
            _aliveEnemiesByPlace.Clear();
            _aliveEnemies.Clear();
            _currentBatchIndex = 0;

            WaveCleared = null;
            BatchCleared = null;
        }

        private bool NoEnemiesLeft()
        {
            return _aliveEnemies.Count == 0;
        }

        public bool IsLastBatch()
        {
            return _waveData.Batches.Count - 1 <= _currentBatchIndex;
        }
    }

    public class WaveSpawner
    {
        public IOrderedEnumerable<Spawner> ClosestSpawners
        {
            get
            {
                return _enemySpawners
                    .Select(spawner => spawner.Value)
                    .OrderBy(spawner => Vector3.Distance(spawner.Position, _gameFactory.Player.transform.position));
            }
        }

        public Spawner ClosestSpawner
        {
            get { return ClosestSpawners.First(); }
        }


        private Dictionary<EnemySpawnerId, Spawner> _enemySpawners;
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

            Spawner CreateSpawnerFrom(SpawnPoint sp)
            {
                Spawner spawner = new Spawner(_gameFactory, sp);
                return spawner;
            }

            _waveState = waveState;
        }

        public IEnumerable<Health> SpawnBatch(Batch batch)
        {
            List<Health> batchEnemies = new();
            foreach (WaveEnemy batchEntry in batch.WaveEnemies)
                batchEnemies.AddRange(SpawnBatchEnemy(batchEntry));
            return batchEnemies;
        }

        private IEnumerable<Health> SpawnBatchEnemy(WaveEnemy batchEntry)
        {
            SingleEnemy singleEnemy = new SingleEnemy(batchEntry);
            for (int i = 0; i < batchEntry.Quantity; i++)
            {
                Spawner spawner = FindAppropriateSpawnerFor(batchEntry);
                Health enemyHealth = spawner.Spawn(singleEnemy);
                _waveState.RegisterEnemy(spawner.Id, enemyHealth);
                yield return enemyHealth;
            }
        }


        private Spawner FindAppropriateSpawnerFor(WaveEnemy batchEntry)
        {
            if (batchEntry.SpawnAt == EnemySpawnerId.Default &&
                (batchEntry.EnemyType == EnemyType.Tentacle || batchEntry.EnemyType == EnemyType.BleedTentacle ||
                 batchEntry.EnemyType == EnemyType.PoisonousTentacle))
                return FindUnoccupiedSpawner();
            else
                return ClosestSpawner;
        }

        private Spawner FindUnoccupiedSpawner()
        {
            foreach (Spawner closestSpawner in ClosestSpawners)
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


    public class WaveSystem : IWaveSystem, IInitializable
    {
        public event Action OnWaveCompleted;
        public event Action<IEnumerable<Health>> OnBatchSpawned;
        public List<EnemyStatsContainer> CurrentWaveEnemies { get; private set; }

        public IWaveScenario CurrentScenario => _currentWaveScenario;
        public IEnumerable<Health> AliveEnemies => _aliveEnemies.Values.SelectMany(enemyList => enemyList);


        private Dictionary<WaveObjective, IWaveScenario> _waveScenarios;
        private IWaveScenario _currentWaveScenario;



        private Dictionary<EnemySpawnerId, Spawner> _enemySpawners;
        private readonly Dictionary<EnemySpawnerId, List<Health>> _aliveEnemies = new();


        private WaveState _currentWaveState;
        private WaveSpawner _waveSpawner;

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

            _waveSpawner = new WaveSpawner(_gameFactory, _sceneDataProvider);
        }


        public void StartWave(int waveIndex)
        {
            WaveData waveData = _wavesData[waveIndex];

            _currentWaveState = new WaveState(waveData);
            _currentWaveState.BatchCleared += OnBatchCleared;
            _currentWaveState.WaveCleared += OnWaveCleared;


            _waveSpawner.Initialize(_currentWaveState);
            _waveSpawner.SpawnBatch(_currentWaveState.CurrentBatchData);



            //int waveIndex = _progressService.ProgressData.WaveIndex;
            //WaveData waveData = _wavesData[waveIndex];
            //_currentWaveScenario = _waveScenarios[waveData.WaveObjective];
            //_currentWaveScenario.Initialize(waveData);
            //_currentWaveScenario.StartWaveScenario();
        }

        public void CompleteWave()
        {
            _progressService.ProgressData.DefeatedWaveIndex++;
            OnWaveCompleted?.Invoke();
            _currentWaveScenario.Dispose();
            _gameStateMachine.SwitchState<WaveCompleteState>();
        }

        private void OnBatchCleared()
        {
            if (!_currentWaveState.IsLastBatch())
            {
                SpawnNextBatch();
            }
        }

        private void OnWaveCleared()
        {
            _gameStateMachine.SwitchState<WaveCompleteState>();
        }

        private void SpawnNextBatch()
        {
            _currentWaveState.ModifyBatchIndex(_currentWaveState.CurrentBatchIndex + 1);
            _waveSpawner.SpawnBatch(_currentWaveState.CurrentBatchData);
        }

        public void KillAllAliveEnemies()
        {
            List<Health> enemies = AliveEnemies.ToList();
            foreach (Health aliveEnemy in enemies)
            {
                aliveEnemy.TakeDamage(100000);
            }
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