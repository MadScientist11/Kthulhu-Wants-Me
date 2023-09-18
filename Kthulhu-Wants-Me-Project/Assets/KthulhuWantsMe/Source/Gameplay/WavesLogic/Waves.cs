﻿using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.WavesLogic
{
    [CreateAssetMenu(menuName = "Create Waves", fileName = "Waves", order = 0)]
    public class Waves : SerializedScriptableObject
    {
        public EnemyScaling BaseEnemyScaling;
        public List<WaveData> WaveData;
        

    }
}