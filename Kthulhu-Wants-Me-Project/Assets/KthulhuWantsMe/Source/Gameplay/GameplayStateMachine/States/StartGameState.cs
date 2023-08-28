using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.GameplayStateMachine.States
{
    public class StartGameState : IGameplayState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IGameFactory _gameFactory;
        private readonly PlayerSpawnPoint _playerSpawnPoint;
        private readonly IInputService _inputService;

        public StartGameState(GameStateMachine gameStateMachine, IGameFactory gameFactory, IInputService inputService,
            PlayerSpawnPoint playerSpawnPoint)
        {
            _inputService = inputService;
            _playerSpawnPoint = playerSpawnPoint;
            _gameFactory = gameFactory;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            _gameFactory.CreatePlayer(_playerSpawnPoint.Position, Quaternion.identity);
            _inputService.SwitchInputScenario(InputScenario.Gameplay);
   
            //_gameStateMachine.SwitchState(GameFlow.StartGameState);
        }

        public void Exit()
        {
        }
    }
}