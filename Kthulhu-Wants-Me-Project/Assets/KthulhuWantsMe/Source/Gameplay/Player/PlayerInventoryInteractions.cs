using System;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services.InputService;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerInventoryInteractions : MonoBehaviour
    {
        [SerializeField] private Transform _itemParent;
        [SerializeField] private PlayerAttack _playerAttack;
        private IInputService _inputService;
        private IInventorySystem _inventorySystem;

        [Inject]
        public void Construct(IInputService inputService, IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
            _inputService = inputService;
            inventorySystem.OnItemSwitched += SwitchItem;
            inventorySystem.OnItemRemoved += UnEquip;
            inventorySystem.OnItemAdded += Equip;
        }

        private void OnDestroy()
        {
            _inventorySystem.OnItemSwitched -= SwitchItem;
            _inventorySystem.OnItemRemoved -= UnEquip;
            _inventorySystem.OnItemAdded -= Equip;
        }

       
        private void Equip(IPickable pickable)
        {
            Debug.Log(pickable);
            pickable.Transform.GetComponent<Rigidbody>().isKinematic = true;
            pickable.Transform.SetParent(_itemParent);
            pickable.Transform.localPosition = Vector3.zero;
        }

        private void UnEquip(IPickable pickable)
        {
            pickable.Transform.SetParent(null);
            pickable.Transform.GetComponent<Rigidbody>().isKinematic = false;
            pickable.Transform.GetComponent<Rigidbody>().AddForce(transform.forward * 150);
        }

        private void SwitchItem(IPickable newItem, IPickable previousItem)
        {
            Debug.Log(newItem);
            newItem?.Transform.gameObject.SetActive(true);
            previousItem?.Transform.gameObject.SetActive(false);
        }
    }
}