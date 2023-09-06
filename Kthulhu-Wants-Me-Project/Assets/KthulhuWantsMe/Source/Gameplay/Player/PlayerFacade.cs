using System;
using Cinemachine;
using KthulhuWantsMe.Source.Gameplay.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerFacade : MonoBehaviour
    {
        [field: SerializeField] public Transform CameraFollowTarget { get; private set; }
        [field: SerializeField] public PlayerLocomotion PlayerLocomotion { get; private set; }
        [field: SerializeField] public PlayerHealth PlayerHealth { get; private set; }
        [field: SerializeField] public TentacleGrabAbilityResponse TentacleGrabAbilityResponse { get; private set; }
        public CinemachineVirtualCamera PlayerVirtualCamera { get; set; }

    }
}