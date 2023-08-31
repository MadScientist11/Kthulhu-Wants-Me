using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Data;
using KthulhuWantsMe.Source.Gameplay.Player;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public class PickableItem : MonoBehaviour, IPickable, IAbilityResponse<PlayerInventoryAbility>
    {
        [field: SerializeField] public virtual PickableData ItemData { get; set; }

        public InteractableData InteractableData => ItemData;

        public Transform Transform => transform;

        public bool Equipped { get; set; }

        public bool Interact()
        {
            return false;
        }

        public void RespondTo(PlayerInventoryAbility ability)
        {
            switch (ability.CurrentAction)
            {
                case InventoryAction.Equip:
                    SwitchToEquippedState(ability);
                    break;
                case InventoryAction.UnEquip:
                    SwitchToUnEquippedState(ability);
                    break;
                case InventoryAction.Hide:
                    SwitchToHiddenState(ability);
                    break;
                case InventoryAction.Show:
                    SwitchToActiveState(ability);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SwitchToEquippedState(PlayerInventoryAbility ability)
        {
            Debug.Log($"Equipped {gameObject.name}");

            GetComponent<Rigidbody>().isKinematic = true;
            transform.SetParent(ability.ItemParent);
            transform.localPosition = ItemData.ItemInHandPosition;
            transform.localRotation = ItemData.ItemInHandRotation;
        }

        private void SwitchToUnEquippedState(PlayerInventoryAbility ability)
        {
            Debug.Log($"UnEquipped {gameObject.name}");
            transform.SetParent(null);
            transform.GetComponent<Rigidbody>().isKinematic = false;
            transform.GetComponent<Rigidbody>().AddForce(transform.forward * 150);
        }

        private void SwitchToActiveState(PlayerInventoryAbility ability)
        {
            gameObject.SetActive(true);
        }

        private void SwitchToHiddenState(PlayerInventoryAbility ability)
        {
            gameObject.SetActive(false);
        }

        [Button]
        private void CollectPositionAndRotation()
        {
            ItemData.ItemInHandPosition = transform.localPosition;
            ItemData.ItemInHandRotation = transform.localRotation;
            EditorUtility.SetDirty(ItemData);
        }
    }
}