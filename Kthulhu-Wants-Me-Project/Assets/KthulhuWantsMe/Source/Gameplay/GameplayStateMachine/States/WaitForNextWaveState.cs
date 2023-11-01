using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services.Audio;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class WaitForNextWaveState : IGameplayState
    {
        private readonly IUIService _uiService;
        private readonly GameplayStateMachine _gameplayStateMachine;
        private IBackgroundMusicPlayer _backgroundMusicPlayer;

        public WaitForNextWaveState(GameplayStateMachine gameplayStateMachine, IUIService uiService, IBackgroundMusicPlayer backgroundMusicPlayer)
        {
            _backgroundMusicPlayer = backgroundMusicPlayer;
            _gameplayStateMachine = gameplayStateMachine;
            _uiService = uiService;
        }
        
        public void Enter()
        {
            _backgroundMusicPlayer.PlayConcernMusic();
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