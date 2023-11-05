using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine;
using KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Gameplay.WaveSystem.Spawn;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    public interface IWaveSystemDirector
    {
        event Action WaveStarted;
        event Action WaveCompleted;
        WaveState CurrentWaveState { get; }
        WaveSpawner WaveSpawner { get; }
        IWaveScenario CurrentWaveScenario { get; }
        void StartWave(int waveIndex);
        void CompleteWave();
        void CompleteWaveAsVictory();
        void CompleteWaveAsFailure();
        void SpawnNextBatch();
    }


    public class WaveSystemDirector : IWaveSystemDirector, IInitializable
    {
        public event Action WaveStarted;
        public event Action WaveCompleted;
        public event Action<IEnumerable<Health>> BatchSpawned;

        public WaveState CurrentWaveState
        {
            get { return _currentWaveState; }
        }

        public WaveSpawner WaveSpawner
        {
            get { return _waveSpawner; }
        }

        public IWaveScenario CurrentWaveScenario
        {
            get { return _currentWaveScenario; }
        }

        private WaveState _currentWaveState;
        private IWaveScenario _currentWaveScenario;

        private WaveSpawner _waveSpawner;

        private bool _batchCleared;
        private bool _waveOngoing;

        private Dictionary<WaveObjective, IWaveScenario> _waveScenarios;

        private readonly IGameFactory _gameFactory;
        private readonly ISceneDataProvider _sceneDataProvider;
        private readonly IDataProvider _dataProvider;
        private readonly GameplayStateMachine.GameplayStateMachine _gameplayStateMachine;
        private readonly IUIService _uiService;
        private CancellationTokenSource _spawnLoopToken;


        public WaveSystemDirector(ISceneDataProvider sceneDataProvider, IDataProvider dataProvider,
            IGameFactory gameFactory,
            IUIService uiService,
            GameplayStateMachine.GameplayStateMachine gameplayStateMachine)
        {
            _uiService = uiService;
            _gameplayStateMachine = gameplayStateMachine;
            _sceneDataProvider = sceneDataProvider;
            _gameFactory = gameFactory;
            _dataProvider = dataProvider;
        }

        public void Initialize()
        {
            _waveSpawner = new WaveSpawner(_gameFactory, _sceneDataProvider, _dataProvider);

            IWaveScenario eliminateAllEnemiesScenario = new EliminateAllEnemiesScenario(this);
            IWaveScenario killTentaclesSpecial = new KillTentaclesSpecialScenario(this, _uiService, _gameFactory);
            IWaveScenario endlessWaveScenario = new EndlessWaveScenario(this, _gameFactory, _uiService, _waveSpawner);

            _waveScenarios = new()
            {
                { WaveObjective.KillAllEnemies, eliminateAllEnemiesScenario },
                { WaveObjective.KillTentaclesSpecial, killTentaclesSpecial },
                { WaveObjective.EndlessWave, endlessWaveScenario }
            };
        }


        public void StartWave(int waveIndex)
        {
            WaveData waveData = _dataProvider.Waves[waveIndex];

            _currentWaveState = new WaveState(waveData);
            _waveSpawner.Initialize(_currentWaveState);

            _currentWaveScenario = _waveScenarios[_currentWaveState.WaveObjective];
            _currentWaveScenario.Initialize();
            _currentWaveScenario.BatchCleared += OnBatchCleared;

            if (_currentWaveState.WaveObjective is not WaveObjective.EndlessWave)
            {
                SpawnBatchLoop().Forget();
            }

            _waveOngoing = true;

            WaveStarted?.Invoke();
        }

        public void CompleteWaveAsFailure()
        {
            if (!_waveOngoing)
                return;

            Debug.Log("Failure");
            _gameplayStateMachine.SwitchState<WaveFailState>();
            CompleteWave();
        }

        public void CompleteWaveAsVictory()
        {
            if (!_waveOngoing)
                return;


            _gameplayStateMachine.SwitchState<WaveVictoryState>();
            CompleteWave();
        }

        public void CompleteWave()
        {
            _uiService.PlayerHUD.HideObjective();

            _spawnLoopToken.Cancel();
            _waveOngoing = false;
            WaveCompleted?.Invoke();
            _currentWaveState.CleanUp();
            _currentWaveScenario.Dispose();
            _currentWaveScenario.BatchCleared -= OnBatchCleared;
        }

        private void OnBatchCleared()
        {
            _batchCleared = true;
        }

        private async UniTaskVoid SpawnBatchLoop()
        {
            _spawnLoopToken = new CancellationTokenSource();
            _spawnLoopToken.RegisterRaiseCancelOnDestroy(_gameFactory.Player);

            // Spawn first wave
            _waveSpawner.SpawnBatchNotified(_currentWaveState.CurrentBatchData);

            while (!_spawnLoopToken.IsCancellationRequested)
            {
                if (_currentWaveState.IsLastBatch())
                {
                    _spawnLoopToken.Cancel();
                    return;
                }

                if (_currentWaveState.CurrentBatchData.WaitForBatchClearance)
                {
                    // Wait until the batch is cleared and spawn next batch then
                    while (!_batchCleared)
                    {
                        await UniTask.Yield();
                    }
                }


                TimeSpan nextBatchDelay = TimeSpan.FromSeconds(_currentWaveState.CurrentBatchData.NextBatchDelay);
                await UniTask.Delay(nextBatchDelay, false, PlayerLoopTiming.Update, _spawnLoopToken.Token);

                SpawnNextBatch();

                _batchCleared = false;
            }
        }

        public void SpawnNextBatch()
        {
            _currentWaveState.ModifyBatchIndex(_currentWaveState.CurrentBatchIndex + 1);
            _waveSpawner.SpawnBatchNotified(_currentWaveState.CurrentBatchData);
        }
    }
}