using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
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
            StartNextWaveCounter(new CancellationTokenSource()).Forget();
        }

        public void Exit()
        {
        }
        
        private async UniTaskVoid StartNextWaveCounter(CancellationTokenSource cancellationToken)
        {
            int countdown = 10;
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay(1000);
                countdown--;
                _uiService.MiscUI.UpdateWaveCountdownText(countdown);
                if (countdown == 0)
                {
                    cancellationToken.Cancel();
                    _gameplayStateMachine.SwitchState<WaveStartState>();
                }
            }
        }
    }
}