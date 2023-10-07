using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class WaveStartState : IGameplayState
    {
        private GameStateMachine _gameStateMachine;
        private readonly IWaveSystem _waveSystem;
        private IProgressService _progressService;

        public WaveStartState(GameStateMachine gameStateMachine, IWaveSystem waveSystem, IProgressService progressService)
        {
            _progressService = progressService;
            _waveSystem = waveSystem;
            _gameStateMachine = gameStateMachine;
        }
        
        public void Enter()
        {
            int newWaveIndex = _progressService.ProgressData.DefeatedWaveIndex + 1;
            _waveSystem.StartWave(newWaveIndex);
        }

        public void Exit()
        {
        }
    }
}