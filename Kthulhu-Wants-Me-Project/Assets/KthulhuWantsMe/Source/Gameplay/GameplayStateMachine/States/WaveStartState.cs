using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class WaveStartState : IGameplayState
    {
        private GameplayStateMachine _gameplayStateMachine;
        private readonly IWaveSystemDirector _waveSystem;
        private IProgressService _progressService;

        public WaveStartState(GameplayStateMachine gameplayStateMachine, IWaveSystemDirector waveSystem, IProgressService progressService)
        {
            _progressService = progressService;
            _waveSystem = waveSystem;
            _gameplayStateMachine = gameplayStateMachine;
        }
        
        public void Enter()
        {
            int newWaveIndex = _progressService.ProgressData.CompletedWaveIndex + 1;
            _waveSystem.StartWave(newWaveIndex);
        }

        public void Exit()
        {
        }
    }
}