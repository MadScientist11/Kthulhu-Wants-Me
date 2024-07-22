using KthulhuWantsMe.Source.Gameplay.WaveSystem.EnemyScaling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    [CreateAssetMenu(menuName = "Create Waves Configuration", fileName = "WavesConfiguration", order = 0)]
    public class WavesConfiguration : SerializedScriptableObject
    {
        public double WaveStartDelay = 1;
        public int MaxEnemiesSpawnAtOnce = 3;
        public EnemyScalingSO BaseEnemyScaling;
        public WavesContainer WavesContainer;


        public WaveData this[int index]
        {
            get
            {
                if (index > WavesContainer.WavesData.Count - 1)
                {
                    Debug.LogError("The wave data index is out of range, returning last wave data in collection.");
                    return WavesContainer.WavesData[^1].WaveData;
                }
                return WavesContainer.WavesData[index].WaveData;
            }
        }
    }
}