using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.UI;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class WaveCompleteState : IGameplayState
    {
        private IUIService _uiService;
        private ThePlayer _player;
        private GameplayStateMachine _gameplayStateMachine;
        private IProgressService _progressService;
        private IInputService _inputService;

        public WaveCompleteState(GameplayStateMachine gameplayStateMachine, IProgressService progressService, 
            IUIService uiService, IInputService inputService,  ThePlayer player)
        {
            _inputService = inputService;
            _progressService = progressService;
            _gameplayStateMachine = gameplayStateMachine;
            _player = player;
            _uiService = uiService;
        }
        public void Enter()
        {
            _progressService.ProgressData.CompletedWaveIndex++;
            _inputService.SwitchInputScenario(InputScenario.UI);
            
            UpgradeWindow window = (UpgradeWindow) _uiService.OpenWindow(WindowId.UpgradeWindow);
           
            //window.Init(OnUpgradePicked);
         
        }

        public void Exit()
        {
        }

        private async void OnUpgradePicked()
        {
            await StartNextWaveCounter(new CancellationTokenSource());
            _gameplayStateMachine.SwitchState<WaveStartState>();
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
                }
            }
        }

    }
}