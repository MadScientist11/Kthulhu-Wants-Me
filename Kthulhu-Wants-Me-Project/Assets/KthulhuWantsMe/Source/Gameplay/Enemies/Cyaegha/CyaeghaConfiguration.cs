using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Cyaegha
{
    [CreateAssetMenu(menuName = "Create CyaeghaConfiguration", fileName = "CyaeghaConfiguration", order = 0)]
    public class CyaeghaConfiguration : EnemyConfiguration
    {
        public float Knockback = 10;
        public float KnockbackTime = .25f;
        public float AttackCooldown;
        public float DelayBeforeJump = 0.75f;
        public float JumpSpeed = 2;
        public float JumpHeight = 2;
        public float LandingSpeed = 5;
    }
}