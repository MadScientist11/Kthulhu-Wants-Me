using System;
using System.Collections.Generic;

namespace KthulhuWantsMe.Source.Gameplay.WavesLogic
{
    [Serializable]
    public class Batch
    {
        public List<WaveEnemy> WaveEnemies;
    }
    [Serializable]
    public class WaveData
    {
        public WaveObjective WaveObjective;
        public List<Batch> Batches;

        // enum
        //
    }
}