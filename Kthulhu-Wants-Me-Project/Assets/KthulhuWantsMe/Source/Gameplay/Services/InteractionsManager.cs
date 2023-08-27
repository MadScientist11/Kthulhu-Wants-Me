using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Infrastructure.Services;
using UnityEngine;
using VContainer.Unity;

namespace KthulhuWantsMe.Source.Gameplay.Interactions
{
    public interface IInteractionsManager
    {
        void AddAvailableInteraction(IInteractable interactable);
        void RemoveAvailableInteraction(IInteractable interactable);
        void ProcessInteraction();
    }

    public class InteractionsManager : IInteractionsManager
    {
        private readonly HashSet<IInteractable> _availableInteractions = new();
        private readonly HashSet<IInteractionHandler> _interactionHandlers = new();

        public InteractionsManager(IGameFactory gameFactory, IInventorySystem inventorySystem)
        {
            _interactionHandlers.Add(new PickableInteractionHandler(gameFactory, inventorySystem));
            _interactionHandlers.Add(new ContainerInteractionHandler(inventorySystem, gameFactory));
        }


        public void AddAvailableInteraction(IInteractable interactable)
        {
            _availableInteractions.Add(interactable);
            Debug.Log("avaliables");
        }

        public void RemoveAvailableInteraction(IInteractable interactable)
        {
            _availableInteractions.Remove(interactable);
        }

        public void ProcessInteraction()
        {
            foreach (IInteractable interactable in _availableInteractions)
            {
                bool successfulInteraction = interactable.Interact();
                
                if(successfulInteraction)
                    return;
            }
        }
    }
}