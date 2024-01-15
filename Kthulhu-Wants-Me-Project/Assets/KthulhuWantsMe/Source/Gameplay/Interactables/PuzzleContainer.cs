using System;
using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items.Story;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using KthulhuWantsMe.Source.Gameplay.Player.State;
using KthulhuWantsMe.Source.Infrastructure;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Interactables
{
    public class PuzzleContainer : InteractableItem, IInjectable
    {
        private Stack<Type> _itemsInOrder;
        private ThePlayer _player;

        [Inject]
        public void Construct(ThePlayer player)
        {
            _player = player;
            
            _itemsInOrder = new Stack<Type>(new[]
            {
                typeof(Key1),
                typeof(Key0),
            });
        }



        public override void RespondTo(PlayerInteractionAbility ability)
        {
            if (_player.Inventory.CurrentItem.Transform.TryGetComponent(_itemsInOrder.Peek(), out Component _))
            {
                if (_player.Inventory.CurrentItem is IConsumable consumable)
                {
                    Debug.Log("Pass 2");
                    _player.Inventory.RemoveItem(consumable);
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