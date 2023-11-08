using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Freya;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Rooms;
using KthulhuWantsMe.Source.Infrastructure.Services;
using MoreMountains.Tools;
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

        private const float _flameSoulSpawnInterval = 10;
        private float _flameSoulLastSpawnTime;

        public LootService(IGameFactory gameFactory, ISceneDataProvider sceneDataProvider, IRoomOverseer roomOverseer)
        {
            _roomOverseer = roomOverseer;
            _gameFactory = gameFactory;
            _navMeshSurface = sceneDataProvider.MapNavMesh;
        }

        public void Tick()
        {
            if (_flameSoulLastSpawnTime + _flameSoulSpawnInterval <= Time.time)
            {
                _flameSoulLastSpawnTime = Time.time;

                IRoom[] roomsByDistance = _roomOverseer.UnlockedRooms
                    .OrderByDescending(room =>
                        Vector3.Distance(room.Transform.position, _roomOverseer.CurrentRoom.Transform.position))
                    .Where(r => r != _roomOverseer.CurrentRoom)
                    .ToArray()
                    .MMShuffle();


                IRoom room = roomsByDistance.FirstOrDefault();

                if (room != null)
                {
                    Vector3 position = room.GetRandomPositionInside()
                        .AddY(GameConstants.SpawnItemsElevation);
                    SpawnBuff<FlameSoul>(position, Quaternion.identity);
                }
            }
        }

        public BuffItem SpawnBuff<T>(Vector3 at, Quaternion rotation) where T : BuffItem
        {
            return _gameFactory.CreateBuffItem<T>(at, rotation);
        }
    }
}