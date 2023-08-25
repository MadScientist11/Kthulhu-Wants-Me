using Cinemachine;
using KthulhuWantsMe.Source.Gameplay.Camera;
using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IGameFactory
    {
        PlayerFacade Player { get; }
        PlayerFacade CreatePlayer(Vector3 position, Quaternion rotation);
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
            CinemachineVirtualCamera playerVirtualCamera = _instantiator.Instantiate(_dataProvider.PlayerConfig.PlayerFPSCameraPrefab);
            playerVirtualCamera.Follow = playerFacade.CameraFollowTarget;
            playerFacade.PlayerVirtualCamera = playerVirtualCamera;
            Player = playerFacade;
            return playerFacade;
        } 
        
    }
}