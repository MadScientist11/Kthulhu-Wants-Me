using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.EntryPoints
{
    public class GameEntryPoint : IInitializable
    {
        private readonly PlayerSpawnPoint _playerSpawnPoint;
        private readonly IGameFactory _gameFactory;

        public GameEntryPoint(IGameFactory gameFactory, PlayerSpawnPoint playerSpawnPoint)
        {
            _gameFactory = gameFactory;
            _playerSpawnPoint = playerSpawnPoint;
        }
        
        public void Initialize()
        {
            _gameFactory.CreatePlayer(_playerSpawnPoint.Position, Quaternion.identity);
        }
    }
}