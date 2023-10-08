using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    [CreateAssetMenu(menuName = "Create WavesContainer", fileName = "WavesContainer", order = 0)]
    public class WavesContainer : SerializedScriptableObject
    {
        public List<WaveTemplate> WavesData;
    }
}