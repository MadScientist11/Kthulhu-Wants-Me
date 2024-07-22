using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Rooms;
using KthulhuWantsMe.Source.Gameplay.WaveSystem;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Utilities.Extensions;
using MoreMountains.Tools;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

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
        private readonly IRoomOverseer _roomOverseer;
        private readonly IWaveSystemDirector _waveSystemDirector;
        private readonly IProgressService _progressService;

        private const float FlameSoulSpawnInterval = 10;
        private const int StartSpawnFlameSoulAfterWave = 3;
        private float _flameSoulLastSpawnTime;

        private readonly List<BuffItem> _loot = new();

        public LootService(IGameFactory gameFactory, 
                           IRoomOverseer roomOverseer, 
                           IWaveSystemDirector waveSystemDirector,
                           IProgressService progressService)
        {
            _progressService = progressService;
            _waveSystemDirector = waveSystemDirector;
            _roomOverseer = roomOverseer;
            _gameFactory = gameFactory;
        }

        public void Tick()
        {
            if (!_waveSystemDirector.WaveOngoing || _progressService.ProgressData.CompletedWaveIndex < StartSpawnFlameSoulAfterWave - 1)
            {
                return;
            }
            
            if (_flameSoulLastSpawnTime + FlameSoulSpawnInterval <= Time.time)
            {
                _flameSoulLastSpawnTime = Time.time;

                IRoom playerCurrentRoom = _roomOverseer.CurrentRoom;

                if (playerCurrentRoom == null)
                {
                    return;
                }
                
                IRoom[] roomsByDistance = _roomOverseer
                    .UnlockedRooms
                    .OrderByDescending(room => Vector3.Distance(room.Transform.position, playerCurrentRoom.Transform.position))
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