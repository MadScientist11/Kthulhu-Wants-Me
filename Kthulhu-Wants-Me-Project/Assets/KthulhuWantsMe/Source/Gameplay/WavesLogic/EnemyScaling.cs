using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.WavesLogic
{
    [CreateAssetMenu(menuName = "Create EnemyScaling", fileName = "EnemyScaling", order = 0)]
    public class EnemyScaling : SerializedScriptableObject
    {
        //health curve
        public Dictionary<StatType, ScaleParameter> StatsScaling;

    }
}