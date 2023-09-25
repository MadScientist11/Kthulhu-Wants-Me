using System;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.SpawnSystem;

namespace KthulhuWantsMe.Source.Gameplay.WavesLogic
{
    [Serializable]
    public class WaveEnemy
    {
        public EnemyType EnemyType;
        public int Quantity;
        public EnemySpawnerId SpawnAt;
    }
}