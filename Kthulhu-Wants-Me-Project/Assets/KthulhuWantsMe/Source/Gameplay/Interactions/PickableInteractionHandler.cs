using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactions
{
    public class PickableInteractionHandler : IInteractionHandler
    {
        private readonly IGameFactory _gameFactory;
        private readonly IInventorySystem _inventorySystem;

        public PickableInteractionHandler(IGameFactory gameFactory, IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
            _gameFactory = gameFactory;
        }
        public void Handle(HashSet<IInteractable> interactables)
        {
            foreach (IInteractable interactable in interactables)
            {
                if(interactable is not IPickable pickable) 
                    continue;
            
                //Play music
                //Inventory add
                //
                if(pickable.Equipped)
                    continue;

               //
               //_inventorySystem.AddItem(pickable, _gameFactory.Player.PlayerActions.PickUp, _gameFactory.Player.PlayerActions.ThrowAway);
                pickable.Equipped = true;
                return;
            }
            
        }
        
        
    }

    public interface IInteractionHandler
    {
        void Handle(HashSet<IInteractable> interactables);
    }
}