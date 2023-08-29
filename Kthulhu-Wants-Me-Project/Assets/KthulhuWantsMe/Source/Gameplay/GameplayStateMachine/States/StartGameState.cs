using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Locations;
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
        private readonly IInputService _inputService;
        private readonly Location _location;

        public StartGameState(GameStateMachine gameStateMachine, IGameFactory gameFactory, IInputService inputService,
            Location location)
        {
            _location = location;
            _inputService = inputService;
            _gameFactory = gameFactory;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            _gameFactory.CreatePlayer(_location.PlayerSpawnPosition, _location.PlayerSpawnRotation);
            _inputService.SwitchInputScenario(InputScenario.Gameplay);
            CreateMonsters();
            //_gameStateMachine.SwitchState(GameFlow.StartGameState);
        }

        public void Exit()
        {
        }

        private void CreateMonsters()
        {
            foreach (LocationEnemyData locationEnemyData in _location.Enemies)
            {
                _gameFactory.CreateEnemy(locationEnemyData.Position, locationEnemyData.Rotation, EnemyType.Tentacle);
            }
        }
    }
}