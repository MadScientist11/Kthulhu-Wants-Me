using System;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.SOData;
using KthulhuWantsMe.Source.Gameplay.Player;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public class PickableItem : MonoBehaviour, IPickable
    {
        [field: SerializeField] public virtual PickableData ItemData { get; set; }

        public InteractableData InteractableData => ItemData;

        public Transform Transform => transform;

        public void RespondTo(PlayerInteractionAbility ability)
        {
            //highlight object or whatever
        }

        public void RespondTo(PlayerHighlightAbility ability)
        {
            switch (ability.HighlightState)
            {
                case HighlightState.Highlight:
                    GetComponent<Renderer>().material.SetFloat("_Outline_Width", 120);
                    GetComponent<Renderer>().material.SetFloat("_Offset_Z", -1);
                    break;
                case HighlightState.CancelHighlight:
                    GetComponent<Renderer>().material.SetFloat("_Outline_Width", 0);
                    GetComponent<Renderer>().material.SetFloat("_Offset_Z", 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            GetComponent<Rigidbody>().isKinematic = true;
            transform.SetParent(ability.ItemParent);
            transform.localPosition = ItemData.ItemInHandPosition;
            transform.localRotation = ItemData.ItemInHandRotation;
        }

        private void SwitchToUnEquippedState(PlayerInventoryAbility ability)
        {
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