using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
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
        private readonly IWaveSystemDirector _waveSystemDirector;
        private readonly ISceneDataProvider _sceneDataProvider;
        private readonly IPauseService _pauseService;

        public WaveVictoryState(GameplayStateMachine gameplayStateMachine, IProgressService progressService,
            IInputService inputService, IUIService uiService, IWaveSystemDirector waveSystemDirector, ISceneDataProvider sceneDataProvider,
            IPauseService pauseService)
        {
            _pauseService = pauseService;
            _sceneDataProvider = sceneDataProvider;
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

            await UniTask.Delay(1000);

            
            UpgradeWindow window = (UpgradeWindow)_uiService.OpenWindow(WindowId.UpgradeWindow);
            window.Init(OnUpgradePicked);
            
            _sceneDataProvider.MapNavMesh.BuildNavMesh();
        }

        public void Exit()
        {
        }

        private void OnUpgradePicked()
        {
            _inputService.SwitchInputScenario(InputScenario.Gameplay);
            _gameplayStateMachine.SwitchState<WaitForNextWaveState>();
        }
    }
}