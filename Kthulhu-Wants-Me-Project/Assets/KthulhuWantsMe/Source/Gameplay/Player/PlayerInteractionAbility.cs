using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public class PlayerInteractionAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private TriggerObserver _interactableZone;

        private void Start()
        {
            _interactableZone.TriggerEnter += HighlightInteractables;
            _interactableZone.TriggerExit += CancelHighlight;
        }

        private void OnDestroy()
        {
            _interactableZone.TriggerEnter -= HighlightInteractables;
            _interactableZone.TriggerExit -= CancelHighlight;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                
            }
        }

        private void HighlightInteractables(Collider interactable)
        {
            
        }

        private void CancelHighlight(Collider interactable)
        {
            
        }
    }
}