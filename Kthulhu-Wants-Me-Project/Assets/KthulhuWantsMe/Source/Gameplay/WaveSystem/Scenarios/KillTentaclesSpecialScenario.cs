using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    public class KillTentaclesSpecialScenario : IWaveScenario
    {
        public event Action BatchCleared;
        public event Action<int> WaveLossTimerTick;
        private CancellationTokenSource _timerToken;

        private int _remainingTentacles;
        private readonly int _waitForEnemiesRetreatDelay = 3;

        private readonly EnemyType[] _additionalEnemies = { EnemyType.Cyeagha, EnemyType.YithCombo1 };
        private int _additionalEnemiesCounter;
        
        private readonly IWaveSystemDirector _waveSystemDirector;
        private readonly IUIService _uiService;
        private readonly IGameFactory _gameFactory;

        public KillTentaclesSpecialScenario(IWaveSystemDirector waveSystemDirector, IUIService uiService, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _uiService = uiService;
            _waveSystemDirector = waveSystemDirector;
        }


        public void Initialize()
        {
            StartWaveLossTimer().Forget();
            _uiService.PlayerHUD._timerUI.gameObject.SwitchOn();
            
            _waveSystemDirector.WaveSpawner.BatchSpawned += OnBatchSpawned;
            _waveSystemDirector.WaveCompleted += OnWaveCompleted;
            _waveSystemDirector.CurrentWaveState.WaveEnemyDied += OnEnemyDied;

        }

        public void Dispose()
        {
            _uiService.PlayerHUD._timerUI.gameObject.SwitchOff();

            _waveSystemDirector.WaveSpawner.BatchSpawned -= OnBatchSpawned;
            _waveSystemDirector.WaveCompleted -= OnWaveCompleted;
            _waveSystemDirector.CurrentWaveState.WaveEnemyDied -= OnEnemyDied;

        }
        
        private void OnEnemyDied(EnemySpawnerId id, Health health)
        {
            if (_waveSystemDirector.CurrentWaveState.NoEnemiesLeft())
            {
                if (_waveSystemDirector.CurrentWaveState.IsLastBatch())
                {
                    BatchCleared?.Invoke();
                    _waveSystemDirector.CompleteWaveAsVictory();
                    return;
                }

                BatchCleared?.Invoke();
            }
        }

        private void OnBatchSpawned(IEnumerable<Health> enemies)
        {
            if (enemies.Any(enemy => enemy.TryGetComponent(out TentacleAIBrain _)))
            {
                _remainingTentacles = enemies
                    .Where(enemy => enemy.TryGetComponent(out TentacleAIBrain _))
                    .ForEach(TrackTentacleDeath).Count();
            }
        }

        private async UniTaskVoid StartWaveLossTimer()
        {
            _timerToken = new CancellationTokenSource();

            TimeSpan tick = TimeSpan.FromSeconds(1);

            int countdown = _waveSystemDirector.CurrentWaveState.CurrentWaveData.TimeConstraint;

            while (!_timerToken.IsCancellationRequested)
            {
                await UniTask.Delay(tick, false, PlayerLoopTiming.Update, _gameFactory.Player.destroyCancellationToken);
                countdown--;

                OnWaveLossTimerTick(countdown);

                if (countdown <= 0)
                {
                    _waveSystemDirector.CompleteWaveAsFailure();
                }
            }
        }

        private void OnWaveLossTimerTick(int countdown)
        {
            WaveData currentWaveData = _waveSystemDirector.CurrentWaveState.CurrentWaveData;
            
            if (countdown % currentWaveData.SpawnEnemyDelay == 0 && _additionalEnemiesCounter < currentWaveData.SpawnedEnemiesCap)
            {
                SpawnAdditionalEnemy();
            }
            _uiService.PlayerHUD._timerUI.UpdateTImerText(countdown);
            WaveLossTimerTick?.Invoke(countdown);
        }

        private void OnWaveCompleted()
        {
            _timerToken.Cancel();
            RetreatAllEnemies();
        }

        private void SpawnAdditionalEnemy()
        {
            _additionalEnemiesCounter++;
            EnemyType randomAdditionalEnemy = _additionalEnemies[Random.Range(0, _additionalEnemies.Length)];
            _waveSystemDirector.WaveSpawner.SpawnEnemyClosestToPlayer(randomAdditionalEnemy);
        }

        private void RetreatAllEnemies()
        {
            for (var index = 0; index < _waveSystemDirector.CurrentWaveState.AliveEnemies.Count; index++)
            {
                Health aliveEnemy = _waveSystemDirector.CurrentWaveState.AliveEnemies[index];
                if (aliveEnemy.TryGetComponent(out IRetreatBehaviour retreatBehaviour))
                {
                    retreatBehaviour.Retreat(true);
                }
            }
        }

        private void TrackTentacleDeath(Health tentacleHealth)
        {
            tentacleHealth.Died += () =>
            {
                _remainingTentacles--;
                if (_remainingTentacles == 0)
                {
                    _waveSystemDirector.CompleteWaveAsVictory();
                }
            };
        }
    }
}