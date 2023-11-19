using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Enemies.Tentacle
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "TentacleConfiguration", fileName = "TentacleConfiguration", order = 0)]
    public class TentacleConfiguration : EnemyConfiguration
    {
        [HideInInspector]
        public float StunWearOffTime;
        [HideInInspector]
        public float TentacleGrabDamage;
        [HideInInspector]
        public float GrabAbilityChance;

        
        [Tooltip("Global Action cooldown")]
        public float ReconsiderationTime;

        [Header("Tentacle Attack")]
        [Tooltip("How close player should be for tentacle to start attacking")]
        public float AttackActivationDistance = 3f;
        
        public float AttackRadius;
        public float AttackEffectiveDistance;
        public float AttackCooldownTime;
        
        [Header("Spell Attack")]
        public float SpellAttackActivationDistance = 3;
        
        [Header("Chase")]
        [Tooltip("Cooldown for tentacle teleport to another portal")]
        public float ChaseCooldown = 10;
        public float ChaseAfterSeconds = 10;
        public float RotationSpeed = 90;

        [Header("Misc")]
        public float AggroRange;
        
        [ShowIf(nameof(EnemyType), EnemyType.PoisonousTentacle)]
        public float PoisonDamagePerSecond;
        [ShowIf(nameof(EnemyType), EnemyType.PoisonousTentacle)]
        public float PoisonEffectDuration;
        [ShowIf(nameof(EnemyType), EnemyType.PoisonousTentacle)]
        public ParticleSystem PoisonEffectPrefab;

      
    }
}