using System.Collections.Generic;
using KthulhuWantsMe.Source.Gameplay.Items;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Interactions
{
    public class InteractionZone : MonoBehaviour
    {
        private IInteractionsManager _interactionsManager;

        [Inject]
        public void Construct(IInteractionsManager interactionsManager)
        {
            _interactionsManager = interactionsManager;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                _interactionsManager.AddAvailableInteraction(interactable);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                _interactionsManager.RemoveAvailableInteraction(interactable);

            }
        }
    }

  
}
