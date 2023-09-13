﻿using System;
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
        GameObject CreateEnemyFromPortal(Vector3 position, Quaternion rotation, EnemyType enemyType);
    }

    public class GameFactory : IGameFactory
    {
        public PlayerFacade Player { get; private set; }
        
        private readonly IObjectResolver _instantiator;
        private readonly IDataProvider _dataProvider;
        private IPortalFactory _portalFactory;

        public GameFactory(IObjectResolver instantiator, IDataProvider dataProvider, IPortalFactory portalFactory)
        {
            _portalFactory = portalFactory;
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

        public GameObject CreateEnemyFromPortal(Vector3 position, Quaternion rotation, EnemyType enemyType)
        {
            Portal portal = _portalFactory.GetOrCreatePortal(position, rotation, enemyType);
            portal.StartEnemySpawn(enemyType);
            return portal.gameObject;
        }
        
        public GameObject CreateEnemy(Vector3 position, Quaternion rotation, EnemyType enemyType)
        {
            //Call state reset
            GameObject instance = enemyType switch
            {
                EnemyType.Tentacle => _instantiator.Instantiate(_dataProvider.TentacleConfig.TentaclePrefab, position,
                    rotation),
                EnemyType.PoisonousTentacle => _instantiator.Instantiate(_dataProvider.PoisonTentacleConfig.TentaclePrefab, position,
                    rotation),
                EnemyType.BleedTentacle => _instantiator.Instantiate(_dataProvider.BleedTentacleConfig.TentaclePrefab, position,
                    rotation),
                EnemyType.Cyeagha => _instantiator.Instantiate(_dataProvider.CyaeghaConfig.Prefab, position,
                    rotation), 
                EnemyType.Yith => _instantiator.Instantiate(_dataProvider.YithConfig.Prefab, position,
                    rotation),
                _ => throw new ArgumentOutOfRangeException(nameof(enemyType), enemyType, null)
            };

            return instance;
        } 
        
        public MinionsSpawnSpell CreateMinionsSpawnSpell(Vector3 position, Quaternion rotation)
        {
            MinionsSpawnSpell instance = _instantiator.Instantiate(_dataProvider.TentacleConfig.MinionsSpawnSpell, position, rotation);

            return instance;
        }

        public BuffItem CreateHealItem(Vector3 position, Quaternion rotation)
        {
            BuffItem instance = _instantiator.Instantiate(_dataProvider.BuffItems.Get<InstaHealthItem>(), position, rotation);
            return instance;
        }

        public T CreatePrefabInjected<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            return _instantiator.Instantiate(prefab, position, rotation);
        } 
       
    }
}