using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.Upgrades;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.UI;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class WaveCompleteState : IGameplayState
    {
        private IUIService _uiService;
        private IWaveSystemDirector _waveSystem;
        private ThePlayer _player;
        private GameStateMachine _gameStateMachine;
        private IProgressService _progressService;

        public WaveCompleteState(GameStateMachine gameStateMachine, IProgressService progressService, IUIService uiService, IWaveSystemDirector waveSystem, ThePlayer player)
        {
            _progressService = progressService;
            _gameStateMachine = gameStateMachine;
            _player = player;
            _waveSystem = waveSystem;
            _uiService = uiService;
        }
        public void Enter()
        {
            _progressService.ProgressData.DefeatedWaveIndex++;
            
            UpgradeWindow window = (UpgradeWindow) _uiService.OpenWindow(WindowId.UpgradeWindow);
            HealthUpgrade healthUpgrade = new HealthUpgrade(_player, 10);
            DamageUpgrade damageUpgrade = new DamageUpgrade(_player, 1);
            EvadeRecoveryUpgrade evadeUpgrade = new EvadeRecoveryUpgrade(_player, .5f);
            window.Init(new List<IUpgrade>()
            {
                healthUpgrade,
                damageUpgrade,
                evadeUpgrade
            }, OnUpgradePicked);
         
        }

        public void Exit()
        {
        }

        private async UniTaskVoid ShowUpgradeWindow()
        {
            UpgradeWindow window = (UpgradeWindow)_uiService.OpenWindow(WindowId.UpgradeWindow);
            HealthUpgrade healthUpgrade = new HealthUpgrade(_player, 10);
            DamageUpgrade damageUpgrade = new DamageUpgrade(_player, 1);
            EvadeRecoveryUpgrade evadeUpgrade = new EvadeRecoveryUpgrade(_player, .5f);
            window.Init(new List<IUpgrade>()
            {
                healthUpgrade,
                damageUpgrade,
                evadeUpgrade
            }, OnUpgradePicked);
        }

        private async void OnUpgradePicked()
        {
            await StartNextWaveCounter(new CancellationTokenSource());
            
            _gameStateMachine.SwitchState<WaveStartState>();
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