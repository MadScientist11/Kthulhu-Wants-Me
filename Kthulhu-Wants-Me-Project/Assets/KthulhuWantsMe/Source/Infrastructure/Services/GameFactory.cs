using Cinemachine;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Gameplay.Spell;
using KthulhuWantsMe.Source.Gameplay.Stats;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace KthulhuWantsMe.Source.Infrastructure.Services
{
    public interface IGameFactory
    {
        PlayerFacade CreatePlayer(Vector3 position, Quaternion rotation);
        GameObject CreateEnemy(Vector3 position, Quaternion rotation, EnemyType enemyType);
        T CreateInjected<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object;
        GameObject CreatePortalWithEnemy(Vector3 position, Quaternion rotation, EnemyType enemyType);
        T CreateInjected<T>(T prefab, Transform parent) where T : Object;
        BuffItem CreateBuffItem<T>(Vector3 position, Quaternion rotation) where T : BuffItem;
    }

    public class GameFactory : IGameFactory
    {
        private readonly IObjectResolver _instantiator;
        private readonly IDataProvider _dataProvider;
        private readonly EnemyStatsProvider _enemyStatsProvider;
        private readonly IProgressService _progressService;
        private readonly IPlayerProvider _playerProvider;

        public GameFactory(IObjectResolver instantiator,
                           IPlayerProvider playerProvider, 
                           IDataProvider dataProvider,
                           EnemyStatsProvider enemyStatsProvider,
                           IProgressService progressService)
        {
            _playerProvider = playerProvider;
            _progressService = progressService;
            _enemyStatsProvider = enemyStatsProvider;
            _dataProvider = dataProvider;
            _instantiator = instantiator;
        }


        public PlayerFacade CreatePlayer(Vector3 position, Quaternion rotation)
        {
            PlayerFacade playerFacade =
                _instantiator.Instantiate(_dataProvider.PlayerConfig.PlayerPrefab, position, rotation);
            CinemachineVirtualCamera playerVirtualCamera =
                _instantiator.Instantiate(_dataProvider.PlayerConfig.PlayerCameraPrefab);
            playerVirtualCamera.Follow = playerFacade.CameraFollowTarget;
            playerFacade.PlayerVirtualCamera = playerVirtualCamera;

            _playerProvider.Set(playerFacade);
            return playerFacade;
        }

        public GameObject CreatePortalWithEnemy(Vector3 position, Quaternion rotation, EnemyType enemyType)
        {
            GameObject enemy = CreateEnemy(position, rotation, enemyType);
            return enemy;
        }

        public GameObject CreateEnemy(Vector3 position, Quaternion rotation, EnemyType enemyType)
        {
            //Call state reset
            GameObject enemyPrefab = _dataProvider.EnemyConfigsProvider.EnemyConfigs[enemyType].Prefab;
            GameObject instance = _instantiator.Instantiate(enemyPrefab, position, rotation);

            if (instance.TryGetComponent(out EnemyStatsContainer enemy))
            {
                EnemyStats enemyStats = _enemyStatsProvider.StatsFor(enemyType, _progressService.ProgressData.CompletedWaveIndex + 1);
                enemy.Initialize(enemyType, enemyStats);
            }

            return instance;
        }
        
        public BuffItem CreateBuffItem<T>(Vector3 position, Quaternion rotation) where T : BuffItem
        {
            BuffItem instance =
                _instantiator.Instantiate(_dataProvider.BuffItems.Get<T>(), position, rotation);
            return instance;
        }
        
        public T CreateInjected<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            return _instantiator.Instantiate(prefab, position, rotation);
        }
        public T CreateInjected<T>(T prefab, Transform parent) where T : Object
        {
            return _instantiator.Instantiate(prefab, parent);
        }
    }
}