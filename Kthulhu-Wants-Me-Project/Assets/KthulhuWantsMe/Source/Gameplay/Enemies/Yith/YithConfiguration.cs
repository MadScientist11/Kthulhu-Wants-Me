using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    [CreateAssetMenu(menuName = "Create YithConfiguration", fileName = "YithConfiguration", order = 0)]
    public class YithConfiguration : EnemyConfiguration
    {
        public float ComboAttackCooldown;
        public float DelayBetweenComboAttacks;
        public float ComboFollowSpeedIncrement;

        public float AttackRadius = 0.75f;
        public float ReconsiderationTime = 2f;
        public float ComboAttackDelay = 0.1f;
        public float ComboAttackDashSpeed = 0.25f;
        public float DashDistance = 15;
    }
}