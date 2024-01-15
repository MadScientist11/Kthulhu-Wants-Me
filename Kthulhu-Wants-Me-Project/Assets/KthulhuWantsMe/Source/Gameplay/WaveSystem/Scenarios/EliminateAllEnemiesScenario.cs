using System;
using KthulhuWantsMe.Source.Gameplay.Entity;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem.Scenarios
{
    public class EliminateAllEnemiesScenario : IWaveScenario
    {
        public event Action BatchCleared;
        
        private readonly IWaveSystemDirector _waveSystemDirector;


        public EliminateAllEnemiesScenario(IWaveSystemDirector waveSystemDirector)
        {
            _waveSystemDirector = waveSystemDirector;
        }

        public void Initialize()
        {
            _waveSystemDirector.CurrentWaveState.WaveEnemyDied += OnEnemyDied;
        }

        public void Dispose()
        {
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
    }
}