using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;

namespace KthulhuWantsMe.Source.Gameplay.StateMachine.States
{
    public class WaveFailState : IGameplayState
    {
        private GameplayStateMachine _gameplayStateMachine;
        private readonly IProgressService _progressService;
        private readonly IUIService _uiService;

        public WaveFailState(GameplayStateMachine gameplayStateMachine, IProgressService progressService, IUIService uiService)
        {
            _uiService = uiService;
            _progressService = progressService;
            _gameplayStateMachine = gameplayStateMachine;
        }
        
        public void Enter()
        {
            _progressService.ProgressData.CompletedWaveIndex++;
            _gameplayStateMachine.SwitchState<WaitForNextWaveState>();
        }

        public void Exit()
        {
        }
    }
}