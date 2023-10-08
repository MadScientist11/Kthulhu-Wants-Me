using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem.EnemyScaling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    [CreateAssetMenu(menuName = "Create Waves", fileName = "Waves", order = 0)]
    public class WavesTemplate : SerializedScriptableObject
    {
        public EnemyScalingSO BaseEnemyScaling;
        public List<WaveTemplate> WavesData;
        
        [TableList]
        [ShowIf("@this.WavesData == null || this.WavesData.Count == 0")]
        public List<WaveData> TestWaveData;
        
        public WaveData this[int index]
        {
            get
            {
                if (WavesData != null && WavesData.Count > 0)
                {
                    return WavesData[index].WaveData;
                }
                return TestWaveData[index];
            }
        }
    }
}