using System.Collections.Generic;
using Cinemachine;
using KthulhuWantsMe.Source.Gameplay.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    [CreateAssetMenu(menuName = GameConstants.MenuPath + "PlayerConfiguration", fileName = "PlayerConfiguration", order = 0)]
    public class PlayerConfiguration : SerializedScriptableObject
    {
        public PlayerFacade PlayerPrefab;
        public CinemachineVirtualCamera PlayerCameraPrefab;


        public Dictionary<StatType, float> BaseStats;

        public float MoveSpeed;
        public Vector3 Gravity = new(0, -30f, 0);
        public float DashSpeed;
        public float AttackRadius;
        public float AttackEffectiveDistance;
        public float AttackStep;
    
        
        public float LungeBaseDamage;
        public float LungeRadius;
        
        
        public float HealthRegenRate = 1;
        
        public float StableMovementSharpness = 15;
        public float OrientationSharpness = 10;
        public float MaxAirMoveSpeed = 10f;
        public float AirAccelerationSpeed = 5f;
        public float Drag = 0.1f;

        public float InvinciblityAfterAttackTime = 1f;
        public bool UnlockAllSkills;
    }
}