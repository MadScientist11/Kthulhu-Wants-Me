using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IGameFactory
    {
        PlayerFacade CreatePlayer(Vector3 position, Quaternion rotation);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IObjectResolver _instantiator;
        private readonly IDataProvider _dataProvider;

        public GameFactory(IObjectResolver instantiator, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _instantiator = instantiator;
        }
        
        public PlayerFacade CreatePlayer(Vector3 position, Quaternion rotation)
        {
           return _instantiator.Instantiate(_dataProvider.PlayerConfig.PlayerPrefab, position, rotation);
        } 
        
    }
}