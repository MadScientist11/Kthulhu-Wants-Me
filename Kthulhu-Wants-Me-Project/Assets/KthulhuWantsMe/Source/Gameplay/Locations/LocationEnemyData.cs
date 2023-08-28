using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Locations
{
    [Serializable]
    public class LocationEnemyData
    {
        public Vector3 Position;
        public EnemyType EnemyType;
    }
}