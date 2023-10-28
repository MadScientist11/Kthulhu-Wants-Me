using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Enemies;
using KthulhuWantsMe.Source.Gameplay.UpgradeSystem;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

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
        public int SpawnEnemyDelay = 3;
        [ShowIf(nameof(WaveSystem.WaveObjective), WaveObjective.KillTentaclesSpecial)]
        public EnemyType EnemyType = EnemyType.Cyeagha;
        [ShowIf(nameof(WaveSystem.WaveObjective), WaveObjective.KillTentaclesSpecial)]
        public int SpawnedEnemiesCap = 25;
    }
}