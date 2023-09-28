﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

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
        [TableColumnWidth(10)]
        [LabelWidth(10)]
        public WaveObjective WaveObjective;
        public List<Batch> Batches;

        // enum
        //
    }
}