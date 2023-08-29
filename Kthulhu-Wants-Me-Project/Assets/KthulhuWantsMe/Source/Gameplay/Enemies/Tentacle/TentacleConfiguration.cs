using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    [CreateAssetMenu(menuName = GameConstants.MenuName + "TentacleConfiguration", fileName = "TentacleConfiguration", order = 0)]
    public class TentacleConfiguration : ScriptableObject
    {
        public GameObject TentaclePrefab;
        
        public float AttackRadius;
        public float AttackEffectiveDistance;
        public float AttackCooldown;
    }
}