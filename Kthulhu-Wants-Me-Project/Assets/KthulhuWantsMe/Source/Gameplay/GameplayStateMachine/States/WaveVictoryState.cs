using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.UI;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class WaveVictoryState : IGameplayState
    {
        private readonly IProgressService _progressService;
        private readonly IInputService _inputService;
        private readonly IUIService _uiService;
        private readonly GameplayStateMachine _gameplayStateMachine;
        private IWaveSystemDirector _waveSystemDirector;

        public WaveVictoryState(GameplayStateMachine gameplayStateMachine, IProgressService progressService, 
            IInputService inputService, IUIService uiService, IWaveSystemDirector waveSystemDirector)
        {
            _waveSystemDirector = waveSystemDirector;
            _gameplayStateMachine = gameplayStateMachine;
            _uiService = uiService;
            _inputService = inputService;
            _progressService = progressService;
        }
        
        public async void Enter()
        {
            _progressService.ProgressData.CompletedWaveIndex++;
            _inputService.SwitchInputScenario(InputScenario.UI);

            await UniTask.Delay(1500);
            
            UpgradeWindow window = (UpgradeWindow) _uiService.OpenWindow(WindowId.UpgradeWindow);
            window.Init(_waveSystemDirector.CurrentWaveState.CurrentWaveData.UpgradeRewards, OnUpgradePicked);
        }

        public void Exit()
        {
            
        }
        
        private async void OnUpgradePicked()
        {
            _inputService.SwitchInputScenario(InputScenario.Gameplay);
            await StartNextWaveCounter(new CancellationTokenSource());
        }
        
        public async UniTask StartNextWaveCounter(CancellationTokenSource cancellationToken)
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