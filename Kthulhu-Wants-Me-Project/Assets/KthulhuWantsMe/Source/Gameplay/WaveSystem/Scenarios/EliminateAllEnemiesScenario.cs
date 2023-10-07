namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    public class EliminateAllEnemiesScenario : IWaveScenario
    {
        private readonly IWaveSystemDirector _waveSystemDirector;

        public EliminateAllEnemiesScenario(IWaveSystemDirector waveSystemDirector)
        {
            _waveSystemDirector = waveSystemDirector;
        }

        public void Initialize()
        {
            _waveSystemDirector.CurrentWaveState.WaveCleared += _waveSystemDirector.CompleteWave;
        }

        public void Dispose()
        {
            _waveSystemDirector.CurrentWaveState.WaveCleared -= _waveSystemDirector.CompleteWave;
        }
    }
}