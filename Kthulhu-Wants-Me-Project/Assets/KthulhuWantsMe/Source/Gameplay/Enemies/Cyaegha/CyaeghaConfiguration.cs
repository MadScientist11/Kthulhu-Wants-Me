using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    [CreateAssetMenu(menuName = "Create CyaeghaConfiguration", fileName = "CyaeghaConfiguration", order = 0)]
    public class CyaeghaConfiguration : EnemyConfiguration
    {
        [MinMaxSlider(0, 20, true)]
        public Vector2 MoveSpeed;
        public float AttackCooldown;
    }
}