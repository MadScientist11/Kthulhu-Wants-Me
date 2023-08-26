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
        [field: SerializeField] public PlayerLocomotionInputProcessor PlayerLocomotionInputProcessor { get; private set; }
        [field: SerializeField] public Transform CameraFollowTarget { get; private set; }
        public CinemachineVirtualCamera PlayerVirtualCamera { get; set; }

        private IInventorySystem _inventorySystem;
        
        [Inject]
        public void Construct(IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
        }
        
        private void Start()
        {
            _inventorySystem.OnItemAdded += PlayerActions.PickUp;
            _inventorySystem.OnItemRemoved += PlayerActions.ThrowAway;
            _inventorySystem.OnItemSwitched += PlayerActions.SwitchItem;
        }

        private void OnDestroy()
        {
            _inventorySystem.OnItemAdded -= PlayerActions.PickUp;
            _inventorySystem.OnItemRemoved -= PlayerActions.ThrowAway;
            _inventorySystem.OnItemSwitched -= PlayerActions.SwitchItem;

        }

        private void OnValidate()
        {
            PlayerLocomotionInputProcessor = GetComponent<PlayerLocomotionInputProcessor>();
            PlayerActions = GetComponent<PlayerActions>();
        }
    }
}