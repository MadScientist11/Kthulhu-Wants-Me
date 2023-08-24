﻿using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    [CreateAssetMenu(menuName = "KhtulhuWantsMe/PlayerConfiguration", fileName = "PlayerConfiguration", order = 0)]
    public class PlayerConfiguration : ScriptableObject
    {
        public PlayerFacade PlayerPrefab;
        
        public float MaxStableMoveSpeed = 10f;
        public float StableMovementSharpness = 15;
        public float OrientationSharpness = 10;
        
        public float MaxAirMoveSpeed = 10f;
        public float AirAccelerationSpeed = 5f;
        public float Drag = 0.1f;
        
        public Vector3 Gravity = new(0, -30f, 0);
    }
}