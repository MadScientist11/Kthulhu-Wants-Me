using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    [Serializable]
    public class EnemyPack
    {
        public EnemyType EnemyType;
        public int Quantity;
        public EnemySpawnerId SpawnAt;
    }
}