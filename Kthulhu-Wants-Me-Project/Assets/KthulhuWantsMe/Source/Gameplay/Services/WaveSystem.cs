using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Services
{
    public enum WaveObjective
    {
        KillAllEnemies = 0,
    }
    
    [Serializable]
    public class WaveData
    {
        public WaveObjective WaveObjective;
        public List<WaveEnemy> WaveEnemies;
        
        // enum
        //
    }

    public class WaveEnemy
    {
        public EnemyType EnemyType;
        public int Quantity;
    }

    public class Waves : ScriptableObject
    {
        public List<WaveData> WavesData;
        
    }
    public class WaveSystem
    {
        
    }
}
