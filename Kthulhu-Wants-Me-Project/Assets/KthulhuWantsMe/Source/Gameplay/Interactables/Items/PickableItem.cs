using System;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.SOData;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public class PickableItem : InteractableItem, IPickable
    {
        public bool Equipped { get; set; }
        [field: SerializeField] public virtual PickableData ItemData { get; set; }
        


        public override void RespondTo(PlayerInteractionAbility ability)
        {
            if (ability.CurrentInteractionAbility is not PlayerInventoryAbility playerInventoryAbility)
                return;
            
            RespondTo(playerInventoryAbility);
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
            Equipped = true;
            GetComponent<Rigidbody>().isKinematic = true;
            transform.SetParent(ability.ItemParent);
            transform.localPosition = ItemData.ItemInHandPosition;
            transform.localRotation = ItemData.ItemInHandRotation;
        }

        private void SwitchToUnEquippedState(PlayerInventoryAbility ability)
        {
            Equipped = false;
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
#if UNITY_EDITOR
        [Button]
        private void CollectPositionAndRotation()
        {
            ItemData.ItemInHandPosition = transform.localPosition;
            ItemData.ItemInHandRotation = transform.localRotation;
            EditorUtility.SetDirty(ItemData);
        }
#endif
    }
}