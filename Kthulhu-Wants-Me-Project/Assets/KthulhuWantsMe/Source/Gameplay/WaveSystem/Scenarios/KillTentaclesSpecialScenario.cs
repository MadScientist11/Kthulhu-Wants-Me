﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    public class KillTentaclesSpecialScenario : IWaveScenario
    {
        public event Action<int> WaveLossTimerTick;
        private CancellationTokenSource _timerToken;

        private int _remainingTentacles;
        private readonly int _waitForEnemiesRetreatDelay = 3;

        private readonly EnemyType[] _additionalEnemies = { EnemyType.Cyeagha, EnemyType.YithCombo1 };
        private int _additionalEnemiesCounter;
        
        private readonly IWaveSystemDirector _waveSystemDirector;
        private readonly IUIService _uiService;

        public KillTentaclesSpecialScenario(IWaveSystemDirector waveSystemDirector, IUIService uiService)
        {
            _uiService = uiService;
            _waveSystemDirector = waveSystemDirector;
        }

        public void Initialize()
        {
            StartWaveLossTimer().Forget();
            
            _waveSystemDirector.WaveSpawner.BatchSpawned += OnBatchSpawned;
            _waveSystemDirector.WaveCompleted += OnWaveCompleted;
        }

        public void Dispose()
        {
            _waveSystemDirector.WaveSpawner.BatchSpawned -= OnBatchSpawned;
            _waveSystemDirector.WaveCompleted -= OnWaveCompleted;

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
                await UniTask.Delay(tick);
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
                Debug.Log("Retreat enemies");

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
                Debug.Log($"Kill spcial tentacle {_remainingTentacles}");
                if (_remainingTentacles == 0)
                {
                    _waveSystemDirector.CompleteWaveAsVictory();
                }
            };
        }
    }
}