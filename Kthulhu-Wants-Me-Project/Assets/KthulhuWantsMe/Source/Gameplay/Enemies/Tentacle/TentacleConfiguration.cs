using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    [CreateAssetMenu(menuName = "Create TentacleConfiguration", fileName = "TentacleConfiguration", order = 0)]
    public class TentacleConfiguration : ScriptableObject
    {
        public TentacleAIBrain TentaclePrefab;
    }
}