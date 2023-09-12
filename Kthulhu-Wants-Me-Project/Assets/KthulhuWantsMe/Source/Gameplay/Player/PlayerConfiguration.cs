using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "PlayerConfiguration", fileName = "PlayerConfiguration", order = 0)]
    public class PlayerConfiguration : ScriptableObject
    {
        [HorizontalLine(color: EColor.Violet)]
        [BoxGroup("Links")]
        public PlayerFacade PlayerPrefab;
        [BoxGroup("Links")]
        public CinemachineVirtualCamera PlayerCameraPrefab;

        [HorizontalLine(color: EColor.Violet)]
        [BoxGroup("Health")]
        public float MaxHealth;
        
        [HorizontalLine(color: EColor.Violet)]
        [BoxGroup("Locomotion")]
        public float MoveSpeed;
        [BoxGroup("Locomotion")]
        public Vector3 Gravity = new(0, -30f, 0);
        [BoxGroup("Locomotion")]
        public float DashStrength;
        [BoxGroup("Locomotion")] 
        public Input InputType;
        [HorizontalLine(color: EColor.Violet)]
        [BoxGroup("Attack")]
        public float AttackRadius;
        [BoxGroup("Attack")]
        public float AttackEffectiveDistance;
        [BoxGroup("Attack")]
        public float BaseDamage;
        [BoxGroup("Attack")]
        public List<Attack> AttackComboSet;
        
        [HorizontalLine(color: EColor.Violet)]
        [BoxGroup("Lunge")]
        public float LungeBaseDamage;
        [BoxGroup("Lunge")]
        public float LungeRadius;
        
        [Foldout("MovementInDepth")]
        public float StableMovementSharpness = 15;
        [Foldout("MovementInDepth")]
        public float OrientationSharpness = 10;
        [Foldout("MovementInDepth")]
        public float MaxAirMoveSpeed = 10f;
        [Foldout("MovementInDepth")]
        public float AirAccelerationSpeed = 5f;
        [Foldout("MovementInDepth")]
        public float Drag = 0.1f;

    }
}