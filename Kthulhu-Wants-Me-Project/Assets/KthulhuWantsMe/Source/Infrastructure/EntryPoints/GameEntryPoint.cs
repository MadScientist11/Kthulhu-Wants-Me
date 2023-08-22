using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.EntryPoints
{
    public class GameEntryPoint : IInitializable
    {
        private readonly PlayerSpawnPoint _playerSpawnPoint;
        private readonly IGameFactory _gameFactory;
        private readonly IInputService _inputService;

        public GameEntryPoint(IGameFactory gameFactory, IInputService inputService, PlayerSpawnPoint playerSpawnPoint)
        {
            _inputService = inputService;
            _gameFactory = gameFactory;
            _playerSpawnPoint = playerSpawnPoint;
        }
        
        public void Initialize()
        {
            _gameFactory.CreatePlayer(_playerSpawnPoint.Position, Quaternion.identity);
            _inputService.SwitchInputScenario(InputScenario.Gameplay);
        }
    }
}