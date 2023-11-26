using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.Audio;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using KthulhuWantsMe.Source.Infrastructure.Services.SceneLoaderService;
using KthulhuWantsMe.Source.Infrastructure.Services.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class StartGameState : IGameplayState
    {
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IInputService _inputService;
        private readonly ISceneDataProvider _sceneDataProvider;
        private readonly IUIService _uiService;
        private readonly ISceneLoader _sceneLoader;
        private readonly ThePlayer _player;
        private readonly IBackgroundMusicPlayer _backgroundMusicPlayer;
        private IPauseService _pauseService;

        public StartGameState(GameplayStateMachine gameplayStateMachine, IGameFactory gameFactory, IInputService inputService,
            ISceneDataProvider sceneDataProvider, IUIService uiService, ISceneLoader sceneLoader, ThePlayer player,
            IBackgroundMusicPlayer backgroundMusicPlayer,
            IPauseService pauseService)
        {
            _pauseService = pauseService;
            _backgroundMusicPlayer = backgroundMusicPlayer;
            _player = player;
            _sceneLoader = sceneLoader;
            _uiService = uiService;
            _sceneDataProvider = sceneDataProvider;
            _inputService = inputService;
            _gameFactory = gameFactory;
            _gameplayStateMachine = gameplayStateMachine;
        }

        public void Enter()
        {
            SpawnPoint playerSpawnPoint = _sceneDataProvider.AllSpawnPoints[SpawnPointType.PlayerSpawnPoint].FirstOrDefault(); 
            
            if (playerSpawnPoint == null)
            {
                Debug.LogError("No player's SpawnPoint in the scene");
                return;
            }
            
            _gameFactory.CreatePlayer(playerSpawnPoint.Position, playerSpawnPoint.Rotation);
            _inputService.SwitchInputScenario(InputScenario.Gameplay);

            _uiService.ShowHUD();
            _backgroundMusicPlayer.PlayConcernMusic();            
            _player.Inventory.OnItemAdded += StartWaveOnWeaponEquip;
            
            _pauseService.ResumeGame();
        }

        public void Exit()
        {
            _player.Inventory.OnItemAdded -= StartWaveOnWeaponEquip;
        }

        private void StartWaveOnWeaponEquip(IPickable item)
        {
            if (item is WeaponItem weaponItem)
            {
                _gameplayStateMachine.SwitchState<WaveStartState>();
            }
        }
    }
}