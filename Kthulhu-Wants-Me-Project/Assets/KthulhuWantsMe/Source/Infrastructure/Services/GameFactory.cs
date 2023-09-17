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

        public GameFactory(IObjectResolver instantiator, IDataProvider dataProvider, IPortalFactory portalFactory, EnemyStatsProvider enemyStatsProvider)
        {
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
            GameObject instance = enemyType switch
            {
                EnemyType.Tentacle => _instantiator.Instantiate(_dataProvider.TentacleConfig.TentaclePrefab, position,
                    rotation),
                EnemyType.PoisonousTentacle => _instantiator.Instantiate(
                    _dataProvider.PoisonTentacleConfig.TentaclePrefab, position,
                    rotation),
                EnemyType.BleedTentacle => _instantiator.Instantiate(_dataProvider.BleedTentacleConfig.TentaclePrefab,
                    position,
                    rotation),
                EnemyType.Cyeagha => _instantiator.Instantiate(
                    _dataProvider.EnemyConfigsProvider.EnemyConfigs[EnemyType.Cyeagha].Prefab, position,
                    rotation),
                EnemyType.Yith => _instantiator.Instantiate(_dataProvider.YithConfig.Prefab, position,
                    rotation),
                _ => throw new ArgumentOutOfRangeException(nameof(enemyType), enemyType, null)
            };

            if (enemyType == EnemyType.Cyeagha)
            {
                instance.GetComponent<Enemy>().Initialize(_enemyStatsProvider.StatsFor(enemyType, 1));
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