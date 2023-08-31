using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.DamageSystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public enum InventoryAction
    {
        Equip = 0,
        UnEquip = 1,
        Hide = 2,
        Show = 3,
    }

    public class PlayerInventoryAbility : MonoBehaviour, IDamageSource, IAbility
    {
        [field: SerializeField] public Transform ItemParent { get; private set; }

        public Transform DamageSourceObject { get; }

        public InventoryAction CurrentAction { get; private set; }


        private IAbilityResponse<PlayerInventoryAbility> _currentResponse;

        private IInventorySystem _inventorySystem;
        private IInputService _inputService;


        [Inject]
        public void Construct(IInventorySystem inventorySystem, IInputService inputService)
        {
            _inputService = inputService;
            _inventorySystem = inventorySystem;
            inputService.GameplayScenario.SwitchItem += SwitchItem;
        }

        private void OnDestroy()
        {
            _inputService.GameplayScenario.SwitchItem -= SwitchItem;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (PhysicsUtility.HitFirst(transform, transform.position, 4, IgnoreEquippedItem, out IPickable item))
                {
                    _inventorySystem.ReplaceItem(item, onItemAdded: Equip, onItemRemoved: UnEquip);
                }
            }
        }

        private void Equip(IPickable item)
        {
            CurrentAction = InventoryAction.Equip;
            item.RespondTo(this);
        }

        private void UnEquip(IPickable item)
        {
            CurrentAction = InventoryAction.UnEquip;
            item.RespondTo(this);
        }

        private void SwitchItem(int index)
        {
            _inventorySystem.SwitchItem(index, (newItem, prevItem) =>
            {
                if(newItem != null)
                    Show(newItem);
                
                if(prevItem != null)
                    Hide(prevItem);
            });
        }

        private void Hide(IPickable item)
        {
            CurrentAction = InventoryAction.Hide;
            item.RespondTo(this);
        }

        private void Show(IPickable item)
        {
            CurrentAction = InventoryAction.Show;
            item.RespondTo(this);
        }

        private bool IgnoreEquippedItem(IPickable item)
        {
            if (item == _inventorySystem.CurrentItem)
                return false;


            return true;
        }
    }
}