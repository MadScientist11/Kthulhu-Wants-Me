using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    [Serializable]
    public class Batch
    {
        public List<EnemyPack> EnemyPack;
        public bool WaitForBatchClearance;
        public int NextBatchDelay;
    }
}