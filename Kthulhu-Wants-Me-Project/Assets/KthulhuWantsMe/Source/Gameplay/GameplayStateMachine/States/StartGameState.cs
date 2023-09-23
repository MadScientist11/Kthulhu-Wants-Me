using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Locations;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class StartGameState : IGameplayState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly IInputService _inputService;
        private ISceneDataProvider _sceneDataProvider;

        public StartGameState(GameStateMachine gameStateMachine, IGameFactory gameFactory, IInputService inputService,
            ISceneDataProvider sceneDataProvider)
        {
            _sceneDataProvider = sceneDataProvider;
            _inputService = inputService;
            _gameFactory = gameFactory;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            SpawnPoint playerSpawnPoint = _sceneDataProvider.AllSpawnPoints[SpawnPointType.PlayerSpawnPoint].FirstOrDefault(); 
            
            if (playerSpawnPoint == null)
            {
                Debug.LogError("No Player SpawnPoint in the scene");
                return;
            }
            
            _gameFactory.CreatePlayer(playerSpawnPoint.Position, playerSpawnPoint.Rotation);
            _inputService.SwitchInputScenario(InputScenario.Gameplay); 
            _gameStateMachine.SwitchState<WaveOngoingState>();
        }

        public void Exit()
        {
        }

      
    }
}