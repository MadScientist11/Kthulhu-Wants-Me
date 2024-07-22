using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.WaveSystem
{
    [CreateAssetMenu(menuName = "Create Wave", fileName = "WaveData", order = 0)]
    public class WaveTemplate : ScriptableObject
    {
        public WaveData WaveData;
    }
}