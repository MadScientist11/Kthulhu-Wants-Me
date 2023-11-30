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
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using MoreMountains.Tools;
using Unity.AI.Navigation;
using UnityEngine;
using VContainer.Unity;
using Vertx.Debugging;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public interface ILootService
    {
        BuffItem SpawnBuff<T>(Vector3 at, Quaternion rotation) where T : BuffItem;
        void DespawnAllLoot();
    }

    public class LootService : ILootService, ITickable
    {
        private readonly IGameFactory _gameFactory;
        private readonly NavMeshSurface _navMeshSurface;
        private readonly IRoomOverseer _roomOverseer;
        private readonly IWaveSystemDirector _waveSystemDirector;

        private const float _flameSoulSpawnInterval = 10;
        private float _flameSoulLastSpawnTime;

        private readonly List<BuffItem> _loot = new();

        public LootService(IGameFactory gameFactory, ISceneDataProvider sceneDataProvider, IRoomOverseer roomOverseer, IWaveSystemDirector waveSystemDirector)
        {
            _waveSystemDirector = waveSystemDirector;
            _roomOverseer = roomOverseer;
            _gameFactory = gameFactory;
            _navMeshSurface = sceneDataProvider.MapNavMesh;
        }

        public void Tick()
        {

            if (!_waveSystemDirector.WaveOngoing)
            {
                return;
            }
            
            if (_flameSoulLastSpawnTime + _flameSoulSpawnInterval <= Time.time)
            {
                _flameSoulLastSpawnTime = Time.time;

                IRoom playerCurrentRoom = _roomOverseer.CurrentRoom;

                if (playerCurrentRoom == null)
                {
                    return;
                }
                
                IRoom[] roomsByDistance = _roomOverseer.UnlockedRooms
                    .OrderByDescending(room =>
                        Vector3.Distance(room.Transform.position, playerCurrentRoom.Transform.position))
                    .Where(r => r != playerCurrentRoom)
                    .ToArray()
                    .MMShuffle();


                IRoom room = roomsByDistance.FirstOrDefault();

                if (room != null)
                {
                    Vector3 position = room
                        .GetRandomPositionInside()
                        .AddY(GameConstants.SpawnItemsElevation);
                    
                    BuffItem flameSoul = SpawnBuff<FlameSoul>(position, Quaternion.identity);
                    _loot.Add(flameSoul);
                }
            }
        }

        public void DespawnAllLoot()
        {
            foreach (BuffItem loot in _loot.Where(t => t != null))
            {
                Object.Destroy(loot.gameObject);
            }

            _loot.Clear();
        }

        public BuffItem SpawnBuff<T>(Vector3 at, Quaternion rotation) where T : BuffItem
        {
            return _gameFactory.CreateBuffItem<T>(at, rotation);
        }
    }
}