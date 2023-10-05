using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    [CreateAssetMenu(menuName = "Create CyaeghaConfiguration", fileName = "CyaeghaConfiguration", order = 0)]
    public class CyaeghaConfiguration : EnemyConfiguration
    {
        public float MoveSpeed;
        public float AttackCooldown;
    }
}