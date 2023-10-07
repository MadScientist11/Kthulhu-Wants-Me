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

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    public class KillTentaclesSpecialScenario : IWaveScenario
    {
        private WaveData _currentWave;
        private CompassUI _compassUI;
        private CancellationTokenSource _timerToken;

        private int _remainingTentacles;
        
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
            _waveSystemDirector.WaveCompleted -= OnWaveCompleted;
        }

        private void OnWaveCompleted()
        {
            _compassUI.Hide();
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

            int countdown = 10;

            while (!_timerToken.IsCancellationRequested)
            {
                await UniTask.Delay(tick);
                countdown--;

                OnWaveLossTimerTick(countdown);

                if (countdown == 0)
                {
                    _timerToken.Cancel();
                    RetreatAllEnemies();
                    _waveSystemDirector.CompleteWave();
                }
            }
        }

        private void OnWaveLossTimerTick(int countdown)
        {
            _uiService.MiscUI.UpdateWaveCountdownText(countdown);
        }
        
        private void RetreatAllEnemies()
        {
            for (var index = 0; index < _waveSystemDirector.CurrentWaveState.AliveEnemies.Count; index++)
            {
                var aliveEnemy = _waveSystemDirector.CurrentWaveState.AliveEnemies[index];
                if (aliveEnemy.TryGetComponent(out RetreatState retreatState))
                {
                    retreatState.Retreat();
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
                    _waveSystemDirector.CompleteWave();
                }
            };
        }
    }
}