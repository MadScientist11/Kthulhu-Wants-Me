using System;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Infrastructure.Services.Audio;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class PlayerDeathState : IGameplayState
    {
        private readonly IBackgroundMusicPlayer _backgroundMusicPlayer;
        private readonly IUIService _uiService;

        public PlayerDeathState(IBackgroundMusicPlayer backgroundMusicPlayer, IUIService uiService)
        {
            _uiService = uiService;
            _backgroundMusicPlayer = backgroundMusicPlayer;
        }
        
        public async void Enter()
        {
            _backgroundMusicPlayer.PlayDefeatMusic();
            await UniTask.Delay(TimeSpan.FromSeconds(3), false, PlayerLoopTiming.Update);
            _uiService.OpenWindow(WindowId.DefeatWindow);

        }

        public void Exit()
        {
            
        }
    }
}