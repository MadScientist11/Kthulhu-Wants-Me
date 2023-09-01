﻿using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items.Story;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Scopes;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Interactables
{
    public class PuzzleContainer : InteractableItem, IInjectable
    {
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


        public override void RespondTo(PlayerInteractionAbility ability)
        {
            if (_inventorySystem.CurrentItem.Transform.TryGetComponent(_itemsInOrder.Peek(), out Component _))
            {
                if (_inventorySystem.CurrentItem is IConsumable consumable)
                {
                    Debug.Log("Pass 2");
                    _inventorySystem.RemoveItem(consumable);
                    consumable.Consume();
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