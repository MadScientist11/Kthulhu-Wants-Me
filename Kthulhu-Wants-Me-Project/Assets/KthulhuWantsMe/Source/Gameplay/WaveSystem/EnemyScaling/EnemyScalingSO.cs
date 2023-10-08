using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.WavesLogic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem.EnemyScaling
{
    [CreateAssetMenu(menuName = "Create EnemyScaling", fileName = "EnemyScaling", order = 0)]
    public class EnemyScalingSO : SerializedScriptableObject
    {
        //health curve
        public Dictionary<StatType, ScaleParameter> StatsScaling;

    }
}