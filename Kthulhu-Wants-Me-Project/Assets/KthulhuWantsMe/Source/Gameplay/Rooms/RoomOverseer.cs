using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using KthulhuWantsMe.Source.Utilities.Extensions;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Rooms
{
    public interface IRoomOverseer
    {
        IEnumerable<IRoom> UnlockedRooms { get; }
        IRoom CurrentRoom { get; }
        void Register(IRoom room);
        void Unregister(IRoom room);
        Vector3 GetRandomPositionInUnlockedRoom();
    }

    public class RoomOverseer : IRoomOverseer
    {
        public IEnumerable<IRoom> UnlockedRooms
        {
            get
            {
                return _rooms.Where(room => !room.Locked);
            }
        }

        public IRoom CurrentRoom
        {
            get
            {
                return UnlockedRooms.FirstOrDefault(current => current.Contains(_playerProvider.Player.transform.position));
            }
        }
        
        private readonly List<IRoom> _rooms = new();
        
        private readonly IPlayerProvider _playerProvider;

        public RoomOverseer(IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public void Register(IRoom room) => 
            _rooms.Add(room);

        public void Unregister(IRoom room) => 
            _rooms.Remove(room);

        public Vector3 GetRandomPositionInUnlockedRoom()
        {
            IRoom unlockedRoom = UnlockedRooms.RandomElement();
            return unlockedRoom.GetRandomPositionInside();
        }
    }
}