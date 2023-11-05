using KthulhuWantsMe.Source.Gameplay.Spell;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "TentacleConfiguration", fileName = "TentacleConfiguration", order = 0)]
    public class TentacleConfiguration : EnemyConfiguration
    {
        public float StunWearOffTime;
        public float TentacleGrabDamage;
        public float GrabAbilityChance;

        public float AggroRange;
        public double AttackRange = 3;
        
        [Tooltip("Action cooldown")]
        public float ReconsiderationTime;

        public float AttackRadius;
        public float AttackEffectiveDistance;
        public float AttackCooldownTime;
        
        public float ChaseCooldown = 10;
        public float ChaseAfterSeconds = 10;
        public float RotationSpeed = 90;

        [ShowIf(nameof(EnemyType), EnemyType.PoisonousTentacle)]
        public float PoisonDamagePerSecond;
        [ShowIf(nameof(EnemyType), EnemyType.PoisonousTentacle)]
        public float PoisonEffectDuration;
        [ShowIf(nameof(EnemyType), EnemyType.PoisonousTentacle)]
        public ParticleSystem PoisonEffectPrefab;
    }
}