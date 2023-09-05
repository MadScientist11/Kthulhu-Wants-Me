using System;
using Cinemachine;
using KinematicCharacterController;
using KthulhuWantsMe.Source.Gameplay.Camera;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Portal;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IGameFactory
    {
        PlayerFacade Player { get; }
        PlayerFacade CreatePlayer(Vector3 position, Quaternion rotation);
        GameObject CreateEnemy(Vector3 position, Quaternion rotation, EnemyType enemyType);
        T CreatePrefabInjected<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object;
    }

    public class GameFactory : IGameFactory
    {
        public PlayerFacade Player { get; private set; }
        
        private readonly IObjectResolver _instantiator;
        private readonly IDataProvider _dataProvider;

        public GameFactory(IObjectResolver instantiator, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _instantiator = instantiator;
        }


        public PlayerFacade CreatePlayer(Vector3 position, Quaternion rotation)
        {
            PlayerFacade playerFacade = _instantiator.Instantiate(_dataProvider.PlayerConfig.PlayerPrefab, position, rotation);
            Player = playerFacade;
            CinemachineVirtualCamera playerVirtualCamera = _instantiator.Instantiate(_dataProvider.PlayerConfig.PlayerCameraPrefab);
            playerVirtualCamera.Follow = playerFacade.CameraFollowTarget;
            playerFacade.PlayerVirtualCamera = playerVirtualCamera;
        
            return playerFacade;
        } 
        
        public GameObject CreateEnemy(Vector3 position, Quaternion rotation, EnemyType enemyType)
        {
            GameObject instance = enemyType switch
            {
                EnemyType.Tentacle => _instantiator.Instantiate(_dataProvider.TentacleConfig.TentaclePrefab, position,
                    rotation),
                EnemyType.Minion => _instantiator.Instantiate(_dataProvider.MinionConfig.MinionPrefab, position,
                    rotation),
                _ => throw new ArgumentOutOfRangeException(nameof(enemyType), enemyType, null)
            };

            return instance;
        } 
        
        public T CreatePrefabInjected<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            return _instantiator.Instantiate(prefab, position, rotation);
        } 
       
    }
}