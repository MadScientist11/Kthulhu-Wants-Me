using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Interactables;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using static UnityEngine.Object;

namespace KthulhuWantsMe.Source.Gameplay.Interactions
{
    public class ContainerInteractionHandler :IInteractionHandler
    {
        private readonly IInventorySystem _inventorySystem;

        private readonly Stack<Type> _itemsInOrder;
        private IGameFactory _gameFactory;

        public ContainerInteractionHandler(IInventorySystem inventorySystem, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _inventorySystem = inventorySystem;
            _itemsInOrder = new Stack<Type>(new[]
            {
                typeof(Key1),
                typeof(Key0),
            });
        } 
        
        public void Handle(HashSet<IInteractable> interactables)
        {
            if (_itemsInOrder.Count == 0)
            {
                Debug.Log("Already unlocked");
                return;

            }

            foreach (IInteractable interactable in interactables)
            {
                if(interactable is not Container container) 
                    continue;


                if (_inventorySystem.CurrentItem.Transform.TryGetComponent(_itemsInOrder.Peek(), out Component _))
                {
                    if (_inventorySystem.CurrentItem is IConsumable consumable)
                    {
                        consumable.RemainingUses--;
                        if (consumable.RemainingUses == 0)
                        {
                           // _inventorySystem.RemoveItem(_inventorySystem.CurrentItem, pickable =>
                           // {
                           //     Destroy(pickable.Transform.gameObject);
                           // });
                        }
                        _itemsInOrder.Pop();
                        
                    }
                }

                if (_itemsInOrder.Count == 0)
                {
                    Debug.Log("Unlocked");
                }
               
                
            }
        }
    }
}