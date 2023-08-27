using KthulhuWantsMe.Source.Gameplay.Interactables.Items;
using KthulhuWantsMe.Source.Gameplay.Interactions;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player
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
                Debug.Log(interactable);
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
