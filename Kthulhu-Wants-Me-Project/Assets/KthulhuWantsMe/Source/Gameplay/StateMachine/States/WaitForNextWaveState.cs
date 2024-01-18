using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.Audio;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;

namespace KthulhuWantsMe.Source.Gameplay.StateMachine.States
{
    public class WaitForNextWaveState : IGameplayState
    {
        private CancellationTokenSource _counterToken;
        
        private readonly IUIService _uiService;
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly IBackgroundMusicPlayer _backgroundMusicPlayer;
        private readonly ILootService _lootService;
        private readonly IGameFactory _gameFactory;
        private IPlayerProvider _playerProvider;

        public WaitForNextWaveState(
            GameplayStateMachine gameplayStateMachine,
            IUIService uiService, 
            IBackgroundMusicPlayer backgroundMusicPlayer,
            ILootService lootService, 
            IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
            _lootService = lootService;
            _backgroundMusicPlayer = backgroundMusicPlayer;
            _gameplayStateMachine = gameplayStateMachine;
            _uiService = uiService;
        }
        
        public void Enter()
        {
            _backgroundMusicPlayer.PlayConcernMusic();
            _lootService.DespawnAllLoot();
            _counterToken = new CancellationTokenSource();
            _counterToken.RegisterRaiseCancelOnDestroy(_playerProvider.Player);
            StartNextWaveCounter(_counterToken).Forget();
        }

        public void Exit()
        {
            _counterToken?.Cancel();
        }
        
        private async UniTaskVoid StartNextWaveCounter(CancellationTokenSource cancellationToken)
        {
            int countdown = 10;
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay(1000);
                countdown--;
                if (countdown == 0)
                {
                    cancellationToken.Cancel();
                    _gameplayStateMachine.SwitchState<WaveStartState>();
                }
            }
        }
    }
}