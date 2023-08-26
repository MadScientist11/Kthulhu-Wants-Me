using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Items;

namespace KthulhuWantsMe.Source.Gameplay.Interactions
{
    public interface IInteractionsManager
    {
        void AddAvailableInteraction(IInteractable interactable);
        void RemoveAvailableInteraction(IInteractable interactable);
    }

    public class InteractionsManager : IInteractionsManager
    {
        
        private HashSet<IInteractable> _availableInteractions = new();

        public void AddAvailableInteraction(IInteractable interactable)
        {
            _availableInteractions.Add(interactable);
        }

        public void RemoveAvailableInteraction(IInteractable interactable)
        {
            _availableInteractions.Remove(interactable);
        }
    }
}