using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.UI.Compass;
using Sirenix.Utilities;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    public class KillTentaclesSpecialScenario : IWaveScenario
    {
        private CompassUI _compassUI;
        private CancellationTokenSource _timerToken;

        private int _remainingTentacles;
        private readonly int _waitForEnemiesRetreatDelay = 3;

        private readonly EnemyType[] _additionalEnemies = { EnemyType.Cyeagha, EnemyType.YithCombo1, EnemyType.YithCombo2, EnemyType.YithCombo3 };
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
            _compassUI = _uiService.MiscUI.GetCompassUI();
            _compassUI.Show();
            
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
            _remainingTentacles = enemies
                .Where(enemy => enemy.TryGetComponent(out TentacleAIBrain _))
                .ForEach(TrackTentacleDeath).Count();
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
            
            if (countdown % currentWaveData.SpawnRandomEnemyEverySeconds == 0 && _additionalEnemiesCounter < currentWaveData.SpawnedEnemiesCap)
            {
                SpawnAdditionalEnemy();
            }
            
            _uiService.MiscUI.UpdateWaveCountdownText(countdown);
        }

        private void OnWaveCompleted()
        {
            _timerToken.Cancel();
            _compassUI.Hide();
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
            Marker marker = new Marker()
            {
                TrackedObject = tentacleHealth.transform
            };
            _compassUI.AddMarker(marker);
            
            tentacleHealth.Died += () =>
            {
                _compassUI.RemoveMarker(marker);
                _remainingTentacles--;

                if (_remainingTentacles == 0)
                {
                    _waveSystemDirector.CompleteWaveAsVictory();
                }
            };
        }
    }
}