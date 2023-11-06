using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Rooms
{
    public interface IRoomOverseer
    {
        void Register(IRoom room);
        void Unregister(IRoom room);
        Vector3 GetRandomPositionInUnlockedRoom();
    }

    public class RoomOverseer : IRoomOverseer
    {
        private readonly List<IRoom> _rooms = new();

        public void Register(IRoom room) => 
            _rooms.Add(room);

        public void Unregister(IRoom room) => 
            _rooms.Remove(room);

        public Vector3 GetRandomPositionInUnlockedRoom()
        {
            IRoom unlockedRoom = _rooms.First(room => !room.Locked);
            return unlockedRoom.GetRandomPositionInside();
        }
    }
}