using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Yith
{
    [CreateAssetMenu(menuName = "Create YithConfiguration", fileName = "YithConfiguration", order = 0)]
    public class YithConfiguration : EnemyConfiguration
    {
        [Header("Simple Attack")] 
        public float AttackRadius = 0.75f;
        public float AttackCooldown = 1;
        public float AttackDelay = 1;

        [Header("Combo Attack")] 
        public int ComboCount = 1;
        public float ComboAttackCooldown;
        public float DelayBetweenComboAttacks;
        public float ComboAttackDelay = 0.1f;
        [FormerlySerializedAs("ComboAttackDashSpeed")] public float ComboAttackDashTime = 0.25f;
        public float DashDistance = 15;
        [MinMaxSlider(2, 20)] public Vector2 ComboAttackTriggerDistance = new(4, 6);

        [Header("Misc")] 
        public float ReconsiderationTime = 2f;
        public double ChaseDistance = 4;
        public float Knockback = 10;
        public float KnockbackTime = .25f;
        public float StunDuration = 1.5f;
    }
}