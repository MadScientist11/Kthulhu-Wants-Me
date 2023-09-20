using KthulhuWantsMe.Source.Gameplay.WavesLogic;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class WaveOngoingState : IGameplayState
    {
        private GameStateMachine _gameStateMachine;
        private IWaveSystem _waveSystem;

        public WaveOngoingState(GameStateMachine gameStateMachine, IWaveSystem waveSystem)
        {
            _waveSystem = waveSystem;
            _gameStateMachine = gameStateMachine;
        }
        
        public void Enter()
        {
            _waveSystem.StartNextWave();
        }

        public void Exit()
        {
        }
    }
}