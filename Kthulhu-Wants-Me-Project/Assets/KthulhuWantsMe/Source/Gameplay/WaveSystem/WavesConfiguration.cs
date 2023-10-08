using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.WaveSystem.EnemyScaling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    [CreateAssetMenu(menuName = "Create Waves Configuration", fileName = "WavesConfiguration", order = 0)]
    public class WavesConfiguration : SerializedScriptableObject
    {
        public EnemyScalingSO BaseEnemyScaling;
        public WavesContainer WavesContainer;
        
        public WaveData this[int index]
        {
            get
            {
                return WavesContainer.WavesData[index].WaveData;
            }
        }
    }
}