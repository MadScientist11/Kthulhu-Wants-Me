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
        WaveState CurrentWaveState { get; }
        void StartWave(int waveIndex);
        void CompleteWave();
    }


    public class WaveSystemDirector : IWaveSystemDirector, IInitializable
    {
        public WaveState CurrentWaveState
        {
            get
            {
                return _currentWaveState;
            }
        }

        private WaveState _currentWaveState;
        private IWaveScenario _currentWaveScenario;

        private WaveSpawner _waveSpawner;
        
        private Dictionary<WaveObjective, IWaveScenario> _waveScenarios;

        private readonly IGameFactory _gameFactory;
        private readonly ISceneDataProvider _sceneDataProvider;
        private readonly Waves _wavesData;
        private readonly GameStateMachine _gameStateMachine;
        private readonly IUIService _uiService;


        public WaveSystemDirector(ISceneDataProvider sceneDataProvider, IDataProvider dataProvider,
            IGameFactory gameFactory,
            IUIService uiService,
            GameStateMachine gameStateMachine)
        {
            _uiService = uiService;
            _gameStateMachine = gameStateMachine;
            _sceneDataProvider = sceneDataProvider;
            _gameFactory = gameFactory;
            _wavesData = dataProvider.Waves;
        }

        public void Initialize()
        {
            IWaveScenario eliminateAllEnemiesScenario = new EliminateAllEnemiesScenario(this);
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

            _currentWaveScenario = _waveScenarios[_currentWaveState.WaveObjective];
            _currentWaveScenario.Initialize();
        }

        public void CompleteWave()
        {
            _currentWaveState.CleanUp();
            _currentWaveScenario.Dispose();
            _gameStateMachine.SwitchState<WaveCompleteState>();
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
            _waveSpawner.SpawnBatch(_currentWaveState.CurrentBatchData);
        }

        public void KillAllAliveEnemies()
        {
            //List<Health> enemies = AliveEnemies.ToList();
            //foreach (Health aliveEnemy in enemies)
            //{
            //    aliveEnemy.TakeDamage(100000);
            //}
        }
    }
}