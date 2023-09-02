using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Services;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;
using VContainer;

namespace KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities
{
    public enum HighlightState
    {
        Highlight = 0,
        CancelHighlight = 1,
    }
    
    public class PlayerHighlightAbility : MonoBehaviour, IAbility
    {
        public HighlightState HighlightState { get; private set; }
        public IInteractable MouseHoverHighlightedInteractable { get; private set; }
        public List<IInteractable> InteractablesInZone => _interactablesInZone;


        [SerializeField] private TriggerObserver _interactionZone;

        private readonly RaycastHit[] _results = new RaycastHit[1];

        private List<IInteractable> _interactablesInZone = new();
        
        private IInventorySystem _inventorySystem;

        [Inject]
        public void Construct(IInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
        }

        private void Start()
        {
            _interactionZone.TriggerEnter += OnInteractableInside;
            _interactionZone.TriggerExit += OnInteractableExit;
        }

        private void OnDestroy()
        {
            _interactionZone.TriggerEnter -= OnInteractableInside;
            _interactionZone.TriggerExit -= OnInteractableExit;
        }

        private void OnInteractableInside(Collider collider)
        {
            if (collider.TryGetComponent(out IInteractable interactable)) 
                _interactablesInZone.Add(interactable);
        }
        private void OnInteractableExit(Collider collider)
        {
            if (collider.TryGetComponent(out IInteractable interactable)) 
                _interactablesInZone.Remove(interactable);
        }
        
        private void Update()
        {
            HighlightObjectOnMouseHover();
            ProcessInteractablesInInteractionZone();
        }

        private void ProcessInteractablesInInteractionZone()
        {
            _interactablesInZone.RemoveAll(i => i == null);
            
            foreach (IInteractable interactable in _interactablesInZone)
            {
                
                if (ShouldHighlightInteractable(interactable))
                {
                    HighlightState = HighlightState.Highlight;
                    interactable.RespondTo(this);
                }
                else
                {
                    HighlightState = HighlightState.CancelHighlight;
                    interactable.RespondTo(this);
                }
            }
        }

        private bool ShouldHighlightInteractable(IInteractable interactable)
        {
            Vector3 playerPos = transform.position;
            Vector3 interactablePos = interactable.Transform.position;
            Vector3 playerLookDirection = transform.forward;

            Vector3 directionTo = interactablePos - playerPos;
            directionTo = directionTo.normalized;

            float lookAtItemIndicator = Vector3.Dot(playerLookDirection, directionTo);

            return lookAtItemIndicator > 0.75f;
        }

        private void HighlightObjectOnMouseHover()
        {
            Ray worldRay = MousePointer.GetWorldRay(UnityEngine.Camera.main);
            if (HitInteractable(worldRay, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    if (InteractableAlreadyHighlighted(interactable) 
                        || InteractableIsEquippedItem(interactable)
                        || InteractableIsOutOfReach(interactable))
                        return;
                    
                    
                    MouseHoverHighlightedInteractable = interactable;
                    HighlightState = HighlightState.Highlight;
                    interactable.RespondTo(this);
                }
            }
            else
            {
                HighlightState = HighlightState.CancelHighlight;
                MouseHoverHighlightedInteractable?.RespondTo(this);
                MouseHoverHighlightedInteractable = null;
            }

            bool InteractableAlreadyHighlighted(IInteractable interactable) => 
                MouseHoverHighlightedInteractable != null && interactable.Transform == MouseHoverHighlightedInteractable.Transform;

            bool InteractableIsOutOfReach(IInteractable interactable) => 
                !_interactablesInZone.Contains(interactable);

            bool InteractableIsEquippedItem(IInteractable interactable)
            {
                if(interactable is IPickable item)
                    return item.Equipped;
                return false;
            }
        }

        private bool HitInteractable(Ray worldRay, out RaycastHit interactable)
        {
            int interactablesMask = LayerMask.GetMask("Interactable", "Item");
            int count = Physics.RaycastNonAlloc(worldRay, _results, 100f, interactablesMask);
            interactable = _results.FirstOrDefault();
            return count > 0;
        }
    }
}