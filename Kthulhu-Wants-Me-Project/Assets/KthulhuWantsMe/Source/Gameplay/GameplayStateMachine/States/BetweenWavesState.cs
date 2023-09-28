using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.UI;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class BetweenWavesState : IGameplayState
    {
        private IUIService _uiService;
        private IWaveSystem _waveSystem;

        public BetweenWavesState(IUIService uiService, IWaveSystem waveSystem)
        {
            _waveSystem = waveSystem;
            _uiService = uiService;
        }
        public async void Enter()
        {
            await StartNextWaveCounter(new CancellationTokenSource());
            _waveSystem.StartNextWave();
        }

        public void Exit()
        {
            
        }
        
        public async UniTask StartNextWaveCounter(CancellationTokenSource cancellationToken)
        {
            int countdown = 10;
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay(1000);
                countdown--;
                _uiService.MiscUIContainer.UpdateWaveCountdownText(countdown);
                if (countdown == 0)
                {
                    cancellationToken.Cancel();
                }
            }
        }

    }
}