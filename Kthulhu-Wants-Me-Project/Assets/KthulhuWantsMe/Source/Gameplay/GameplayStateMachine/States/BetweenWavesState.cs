using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.Upgrades;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using KthulhuWantsMe.Source.UI;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class BetweenWavesState : IGameplayState
    {
        private IUIService _uiService;
        private IWaveSystem _waveSystem;
        private ThePlayer _player;

        public BetweenWavesState(IUIService uiService, IWaveSystem waveSystem, ThePlayer player)
        {
            _player = player;
            _waveSystem = waveSystem;
            _uiService = uiService;
        }
        public async void Enter()
        {
            UpgradeWindow window = (UpgradeWindow)await _uiService.OpenWindow(WindowId.UpgradeWindow);
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

        private async void OnUpgradePicked()
        {
            await StartNextWaveCounter(new CancellationTokenSource());
            _waveSystem.StartNextWave();
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