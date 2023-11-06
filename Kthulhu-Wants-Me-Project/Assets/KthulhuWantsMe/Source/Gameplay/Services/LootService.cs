using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Freya;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Rooms;
using KthulhuWantsMe.Source.Infrastructure.Services;
using Unity.AI.Navigation;
using UnityEngine;
using VContainer.Unity;
using Vertx.Debugging;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface ILootService
    {
        BuffItem SpawnBuff<T>(Vector3 at, Quaternion rotation) where T : BuffItem;
    }

    public class LootService : ILootService, ITickable
    {
        private readonly IGameFactory _gameFactory;
        private readonly NavMeshSurface _navMeshSurface;
        private readonly IRoomOverseer _roomOverseer;

        private const float _flameSoulSpawnInterval = 5;
        private float _flameSoulLastSpawnTime;

        public LootService(IGameFactory gameFactory, ISceneDataProvider sceneDataProvider, IRoomOverseer roomOverseer)
        {
            _roomOverseer = roomOverseer;
            _gameFactory = gameFactory;
            _navMeshSurface = sceneDataProvider.MapNavMesh;
        }
        
        public void Tick()
        {
            return;
            if (_flameSoulLastSpawnTime + _flameSoulSpawnInterval <= Time.time)
            {
                _flameSoulLastSpawnTime = Time.time;
                SpawnBuff<FlameSoul>(_roomOverseer.GetRandomPositionInUnlockedRoom(), Quaternion.identity);
            }
        }

        public BuffItem SpawnBuff<T>(Vector3 at, Quaternion rotation) where T : BuffItem
        {
            return _gameFactory.CreateBuffItem<T>(at, rotation);
        }
    }
}