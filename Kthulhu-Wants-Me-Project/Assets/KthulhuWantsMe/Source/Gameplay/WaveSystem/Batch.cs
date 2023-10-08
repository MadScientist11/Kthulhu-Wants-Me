using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    [Serializable]
    public class Batch
    {
        public List<EnemyPack> EnemyPack;
        public int NextBatchDelay;
    }
}