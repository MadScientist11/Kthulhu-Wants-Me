using System;
using System.Collections.Generic;

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