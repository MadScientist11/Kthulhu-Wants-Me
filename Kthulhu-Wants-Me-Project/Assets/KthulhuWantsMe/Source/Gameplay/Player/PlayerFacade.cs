using System;
using Cinemachine;
using KthulhuWantsMe.Source.Gameplay.Services;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerFacade : MonoBehaviour
    {
        [field: SerializeField] public PlayerActions PlayerActions { get; private set; }
        [field: SerializeField] public PlayerLocomotion PlayerLocomotion { get; private set; }
        [field: SerializeField] public Transform CameraFollowTarget { get; private set; }
        public CinemachineVirtualCamera PlayerVirtualCamera { get; set; }

        private IInventorySystem _inventorySystem;
        
        [Inject]
        public void Construct(IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
        }

        private void OnValidate()
        {
            PlayerLocomotion = GetComponent<PlayerLocomotion>();
            PlayerActions = GetComponent<PlayerActions>();
        }
    }
}