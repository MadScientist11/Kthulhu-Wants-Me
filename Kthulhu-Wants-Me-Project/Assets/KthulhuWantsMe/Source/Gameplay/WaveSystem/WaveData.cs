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
        
        [ShowIf(nameof(WaveSystem.WaveObjective), WaveObjective.KillTentaclesSpecial)]
        public int TimeConstraint = 100;
        [ShowIf(nameof(WaveSystem.WaveObjective), WaveObjective.KillTentaclesSpecial)]
        public int SpawnRandomEnemyEverySeconds = 3;
        [ShowIf(nameof(WaveSystem.WaveObjective), WaveObjective.KillTentaclesSpecial)]
        public int SpawnedEnemiesCap = 25;
        
        public List<UpgradeData> UpgradeRewards;
    }
}