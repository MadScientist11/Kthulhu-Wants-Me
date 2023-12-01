using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using MoreMountains.Feedbacks;
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

    public class PlayerInventoryAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private MMFeedbacks _equipFeedback;
        [field: SerializeField] public Transform ItemParent { get; private set; }

        public Transform DamageSourceObject { get; }

        public InventoryAction CurrentAction { get; private set; }


        private IAbilityResponse<PlayerInventoryAbility> _currentResponse;

        private IInputService _inputService;
        private ThePlayer _player;

        [Inject]
        public void Construct(ThePlayer player, IInputService inputService)
        {
            _player = player;
            _inputService = inputService;
           // inputService.GameplayScenario.SwitchItem += SwitchItem;
        }

        private void OnDestroy()
        {
            //_inputService.GameplayScenario.SwitchItem -= SwitchItem;
        }

        public bool PickUpItem()
        {
            if (PhysicsUtility.HitFirst(transform, transform.position, 3, IgnoreEquippedItem, out IPickable item))
            {
                _player.Inventory.ReplaceItem(item, onItemAdded: Equip, onItemRemoved: UnEquip);
                return true;
            }

            return false;
        }

        public void PickUpItem(IPickable item)
        {
            _player.Inventory.ReplaceItem(item, onItemAdded: Equip, onItemRemoved: UnEquip);
        }


        private void Equip(IPickable item)
        {
            CurrentAction = InventoryAction.Equip;
            _equipFeedback?.PlayFeedbacks();
            item.RespondTo(this);
        }

        private void UnEquip(IPickable item)
        {
            CurrentAction = InventoryAction.UnEquip;
            item.RespondTo(this);
        }

        private void SwitchItem(int index)
        {
            _player.Inventory.SwitchItem(index, (newItem, prevItem) =>
            {
                if (newItem != null)
                    Show(newItem);

                if (prevItem != null)
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
            if (item == _player.Inventory.CurrentItem)
                return false;


            return true;
        }
    }
}