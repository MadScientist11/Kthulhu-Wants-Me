using System;
using Cinemachine;
using KinematicCharacterController;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Camera;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.PortalsLogic;
using KthulhuWantsMe.Source.Gameplay.Spell;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using KthulhuWantsMe.Source.Infrastructure.Services.DataProviders;
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
        MinionsSpawnSpell CreateMinionsSpawnSpell(Vector3 position, Quaternion rotation);
        BuffItem CreateHealItem(Vector3 position, Quaternion rotation);
        Portal CreatePortalWithEnemy(Vector3 position, Quaternion rotation, EnemyType enemyType);
    }

    public class GameFactory : IGameFactory
    {
        public PlayerFacade Player { get; private set; }

        private readonly IObjectResolver _instantiator;
        private readonly IDataProvider _dataProvider;
        private IPortalFactory _portalFactory;
        private EnemyStatsProvider _enemyStatsProvider;
        private IProgressService _progressService;

        public GameFactory(IObjectResolver instantiator, IDataProvider dataProvider, IPortalFactory portalFactory,
            EnemyStatsProvider enemyStatsProvider, IProgressService progressService)
        {
            _progressService = progressService;
            _enemyStatsProvider = enemyStatsProvider;
            _portalFactory = portalFactory;
            _dataProvider = dataProvider;
            _instantiator = instantiator;
        }


        public PlayerFacade CreatePlayer(Vector3 position, Quaternion rotation)
        {
            PlayerFacade playerFacade =
                _instantiator.Instantiate(_dataProvider.PlayerConfig.PlayerPrefab, position, rotation);
            Player = playerFacade;
            CinemachineVirtualCamera playerVirtualCamera =
                _instantiator.Instantiate(_dataProvider.PlayerConfig.PlayerCameraPrefab);
            playerVirtualCamera.Follow = playerFacade.CameraFollowTarget;
            playerFacade.PlayerVirtualCamera = playerVirtualCamera;

            return playerFacade;
        }

        public Portal CreatePortalWithEnemy(Vector3 position, Quaternion rotation, EnemyType enemyType)
        {
            Portal portal = _portalFactory.GetOrCreatePortal(position, rotation, enemyType);
            portal.StartEnemySpawn(enemyType);
            return portal;
        }

        public GameObject CreateEnemy(Vector3 position, Quaternion rotation, EnemyType enemyType)
        {
            //Call state reset
            GameObject enemyPrefab = _dataProvider.EnemyConfigsProvider.EnemyConfigs[enemyType].Prefab;
            GameObject instance = _instantiator.Instantiate(enemyPrefab, position, rotation);

            if (instance.TryGetComponent(out Enemy enemy))
            {
                EnemyStats enemyStats = _enemyStatsProvider.StatsFor(enemyType, _progressService.ProgressData.WaveIndex);
                enemy.Initialize(enemyStats);
            }


            return instance;
        }

        public MinionsSpawnSpell CreateMinionsSpawnSpell(Vector3 position, Quaternion rotation)
        {
            //MinionsSpawnSpell instance = _instantiator.Instantiate(null, position, rotation);

            return null;
        }

        public BuffItem CreateHealItem(Vector3 position, Quaternion rotation)
        {
            BuffItem instance =
                _instantiator.Instantiate(_dataProvider.BuffItems.Get<InstaHealthItem>(), position, rotation);
            return instance;
        }

        public T CreatePrefabInjected<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            return _instantiator.Instantiate(prefab, position, rotation);
        }
    }
}