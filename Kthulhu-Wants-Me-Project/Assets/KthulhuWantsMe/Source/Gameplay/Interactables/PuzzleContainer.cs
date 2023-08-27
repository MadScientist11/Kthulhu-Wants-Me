using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Interactables.Data;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public class PuzzleContainer : MonoBehaviour, IInteractable, IInjectable
    {
        public InteractableData InteractableData { get; }
        public Transform Transform => transform;
        private Stack<Type> _itemsInOrder;
        private IInventorySystem _inventorySystem;

        [Inject]
        public void Construct(IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
            
            _itemsInOrder = new Stack<Type>(new[]
            {
                typeof(Key1),
                typeof(Key0),
            });
        }

        public bool Interact()
        {
            bool isSuccess = false;
            
            if (_inventorySystem.CurrentItem.Transform.TryGetComponent(_itemsInOrder.Peek(), out Component _))
            {
                if (_inventorySystem.CurrentItem is IConsumable consumable)
                {
                    Debug.Log("Pass 2");

                    consumable.Consume();
                    _itemsInOrder.Pop();
                    isSuccess = true;
                }
            }

            if (_itemsInOrder.Count == 0)
            {
                Debug.Log("Unlocked");
            }

            return isSuccess;
        }
    }
}