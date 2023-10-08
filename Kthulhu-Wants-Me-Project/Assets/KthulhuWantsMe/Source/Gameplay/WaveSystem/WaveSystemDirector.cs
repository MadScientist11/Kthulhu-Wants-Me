using System;
using System.Collections.Generic;
using System.Linq;
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
        event Action WaveCompleted;
        WaveState CurrentWaveState { get; }
        WaveSpawner WaveSpawner { get; }
        void StartWave(int waveIndex);
        void CompleteWave();
    }


    public class WaveSystemDirector : IWaveSystemDirector, IInitializable
    {

        public event Action WaveCompleted;
        public event Action<IEnumerable<Health>> BatchSpawned;

        public WaveState CurrentWaveState
        {
            get
            {
                return _currentWaveState;
            }
        }
        
        public WaveSpawner WaveSpawner
        {
            get
            {
                return _waveSpawner;
            }
        }

        private WaveState _currentWaveState;
        private IWaveScenario _currentWaveScenario;

        private WaveSpawner _waveSpawner;
        
        private Dictionary<WaveObjective, IWaveScenario> _waveScenarios;

        private readonly IGameFactory _gameFactory;
        private readonly ISceneDataProvider _sceneDataProvider;
        private readonly Waves _wavesData;
        private readonly GameplayStateMachine.GameplayStateMachine _gameplayStateMachine;
        private readonly IUIService _uiService;


        public WaveSystemDirector(ISceneDataProvider sceneDataProvider, IDataProvider dataProvider,
            IGameFactory gameFactory,
            IUIService uiService,
            GameplayStateMachine.GameplayStateMachine gameplayStateMachine)
        {
            _uiService = uiService;
            _gameplayStateMachine = gameplayStateMachine;
            _sceneDataProvider = sceneDataProvider;
            _gameFactory = gameFactory;
            _wavesData = dataProvider.Waves;
        }

        public void Initialize()
        {
            IWaveScenario eliminateAllEnemiesScenario = new EliminateAllEnemiesScenario(this);
            IWaveScenario killTentaclesSpecial = new KillTentaclesSpecialScenario(this, _uiService);
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
            
            _currentWaveScenario = _waveScenarios[_currentWaveState.WaveObjective];
            _currentWaveScenario.Initialize();

            _waveSpawner.Initialize(_currentWaveState);
            _waveSpawner.SpawnBatchNotified(_currentWaveState.CurrentBatchData);
        }

        public void CompleteWave()
        {
            WaveCompleted?.Invoke();
            _currentWaveState.CleanUp();
            _currentWaveScenario.Dispose();
            _gameplayStateMachine.SwitchState<WaveCompleteState>();
        }

        private void OnBatchCleared()
        {
            if (!_currentWaveState.IsLastBatch())
            {
                SpawnNextBatch().Forget();
            }
        }

        private void OnWaveCleared()
        {
        }

        private async UniTaskVoid SpawnNextBatch()
        {
            TimeSpan nextBatchDelay = TimeSpan.FromSeconds(_currentWaveState.CurrentBatchData.NextBatchDelay);
            await UniTask.Delay(nextBatchDelay);
            _currentWaveState.ModifyBatchIndex(_currentWaveState.CurrentBatchIndex + 1);
            _waveSpawner.SpawnBatchNotified(_currentWaveState.CurrentBatchData);
        }
    }
}