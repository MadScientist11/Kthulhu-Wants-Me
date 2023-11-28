using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.Audio;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class WaveStartState : IGameplayState
    {
        private GameplayStateMachine _gameplayStateMachine;
        private readonly IWaveSystemDirector _waveSystem;
        private IProgressService _progressService;
        private ISceneDataProvider _sceneDataProvider;
        private IBackgroundMusicPlayer _backgroundMusicPlayer;

        public WaveStartState(GameplayStateMachine gameplayStateMachine, IWaveSystemDirector waveSystem, IProgressService progressService, 
            ISceneDataProvider sceneDataProvider, IBackgroundMusicPlayer backgroundMusicPlayer)
        {
            _backgroundMusicPlayer = backgroundMusicPlayer;
            _sceneDataProvider = sceneDataProvider;
            _progressService = progressService;
            _waveSystem = waveSystem;
            _gameplayStateMachine = gameplayStateMachine;
        }
        
        public void Enter()
        {
            int newWaveIndex = _progressService.ProgressData.CompletedWaveIndex + 1;
            _waveSystem.StartWave(newWaveIndex);
            _backgroundMusicPlayer.PlayBattleMusic();
            _sceneDataProvider.MapNavMesh.BuildNavMesh();

        }

        public void Exit()
        {
        }
    }
}