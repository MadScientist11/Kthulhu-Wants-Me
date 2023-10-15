using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    [CreateAssetMenu(menuName = "Create YithConfiguration", fileName = "YithConfiguration", order = 0)]
    public class YithConfiguration : EnemyConfiguration
    {
        public float ComboAttackCooldown;
        public float DelayBetweenComboAttacks;
        public float ComboFollowSpeedIncrement;
        
        [MinMaxSlider(0, 20, true)]
        public Vector2 MoveSpeed;
    }
}