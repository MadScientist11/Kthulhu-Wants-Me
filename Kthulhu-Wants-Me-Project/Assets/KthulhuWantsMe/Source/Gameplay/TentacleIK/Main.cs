using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.TentacleIK
{
    public class Main : MonoBehaviour
    {
        private IGameFactory _gameFactory;
        private PlayerSpawnPoint _playerSpawnPoint;
        private IReadOnlyList<IInitializableService> _services;

        [Inject]
        public void Construct(IReadOnlyList<IInitializableService> services, IGameFactory gameFactory, PlayerSpawnPoint playerSpawnPoint)
        {
            _services = services;
            Debug.Log(gameFactory);
            Debug.Log(playerSpawnPoint);
            _playerSpawnPoint = playerSpawnPoint;
            _gameFactory = gameFactory;
        }

        private async void Start()
        {
            foreach (IInitializableService service in _services)
            {
               await service.Initialize();
            }
            Debug.Log("Start");
            _gameFactory.CreatePlayer(_playerSpawnPoint.Position, Quaternion.identity);
        }
    }
}
