using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using Sirenix.OdinInspector;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    [Serializable]
    public class WaveData
    {
        [TableColumnWidth(10)]
        [LabelWidth(10)]
        public WaveObjective WaveObjective;
        public List<Batch> Batches;
        
        [ShowIf("WaveObjective", WaveObjective.KillTentaclesSpecial)]
        public int TimeConstraint;
        
        public List<UpgradeData> UpgradeRewards;
    }
}