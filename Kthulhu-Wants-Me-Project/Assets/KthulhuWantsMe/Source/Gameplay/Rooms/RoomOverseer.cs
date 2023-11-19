using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer;

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
                return UnlockedRooms.FirstOrDefault(current => current.Contains(_gameFactory.Player.transform.position));
            }
        }
        
        private readonly List<IRoom> _rooms = new();
        
        private readonly IGameFactory _gameFactory;

        public RoomOverseer(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
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