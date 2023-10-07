using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.UI.Compass;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    public class KillTentaclesSpecialScenario : IWaveScenario
    {
        private readonly WaveSystemDirector _waveSystemDirector;
        private readonly IUIService _uiService;
        private readonly IGameFactory _gameFactory;
        private WaveData _currentWave;
        private CompassUI _compassUI;
        private CancellationTokenSource _timerToken;

        private int _remainingTentacles;

        public KillTentaclesSpecialScenario(WaveSystemDirector waveSystemDirector, IUIService uiService, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _uiService = uiService;
            _waveSystemDirector = waveSystemDirector;
        }


        public void Initialize()
        {
            
        }

        public void Dispose()
        {
           
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

            foreach (Health aliveEnemy in _waveSystemDirector.CurrentWaveState.AliveEnemies)
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
                            _waveSystemDirector.KillAllAliveEnemies();
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
                    List<Health> waveSystemAliveEnemies = _waveSystemDirector.CurrentWaveState.AliveEnemies.ToList();
                    foreach (Health aliveEnemy in waveSystemAliveEnemies)
                    {
                        if (aliveEnemy.TryGetComponent(out RetreatState retreatState))
                        {
                            retreatState.Retreat();
                        }
                    }

                    await UniTask.Delay(2000);
                    _waveSystemDirector.CompleteWave();
                }
            }
        }
    }
}