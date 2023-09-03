using System.Collections.Generic;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;

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
        public IInteractable MouseHoverInteractable { get; private set; }
        public List<IInteractable> InteractablesInZone => _interactablesInZone;


        [SerializeField] private TriggerObserver _interactionZone;

        private readonly RaycastHit[] _results = new RaycastHit[1];

        private List<IInteractable> _interactablesInZone = new();

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
            HighlightOnMouseHover();
            ProcessInteractablesInInteractionZone();
        }

        private void ProcessInteractablesInInteractionZone()
        {
            HandleEquippedItems();

            foreach (IInteractable interactable in _interactablesInZone)
            {
                HighlightState newState = InteractableIsVisible(interactable)
                    ? HighlightState.Highlight
                    : HighlightState.CancelHighlight;
                
                ChangeStateFor(interactable, newState);
            }
        }

        private void HandleEquippedItems()
        {
            _interactablesInZone.RemoveAll(i =>
            {
                if (i is IPickable { Equipped: true } item)
                {
                    ChangeStateFor(item, HighlightState.CancelHighlight);
                    return true;
                }

                return false;
            });
        }

        private void HighlightOnMouseHover()
        {
            Ray worldRay = MousePointer.GetWorldRay(UnityEngine.Camera.main);
            if (HitInteractable(worldRay, out IInteractable interactable))
            {
                if (InteractableAlreadyHighlighted(interactable)
                    || InteractableIsEquippedItem(interactable)
                    || InteractableIsOutOfReach(interactable))
                    return;

                MouseHoverInteractable = interactable;
                ChangeStateFor(MouseHoverInteractable, HighlightState.Highlight);
            }
            else
            {
                if (MouseHoverInteractable != null)
                    ChangeStateFor(MouseHoverInteractable, HighlightState.CancelHighlight);

                MouseHoverInteractable = null;
            }

            bool InteractableAlreadyHighlighted(IInteractable i) =>
                MouseHoverInteractable != null && i.Transform == MouseHoverInteractable.Transform;

            bool InteractableIsOutOfReach(IInteractable i) =>
                !_interactablesInZone.Contains(i);

            bool InteractableIsEquippedItem(IInteractable i)
            {
                if (i is IPickable item)
                    return item.Equipped;
                return false;
            }
        }

        private void ChangeStateFor(IInteractable interactable, HighlightState highlightState)
        {
            HighlightState = highlightState;
            interactable.RespondTo(this);
        }

        private bool InteractableIsVisible(IInteractable interactable)
        {
            Vector3 playerPos = transform.position;
            Vector3 interactablePos = interactable.Transform.position;
            Vector3 playerLookDirection = transform.forward;

            Vector3 directionTo = interactablePos - playerPos;
            directionTo = directionTo.normalized;

            float lookAtItemIndicator = Vector3.Dot(playerLookDirection, directionTo);

            return lookAtItemIndicator > 0.75f;
        }

        private bool HitInteractable(Ray worldRay, out IInteractable interactable)
        {
            int interactablesMask = LayerMask.GetMask("Interactable", "Item");
            int hitCount = Physics.RaycastNonAlloc(worldRay, _results, 100f, interactablesMask);
            RaycastHit hit = _results.FirstOrDefault();

            if (hitCount > 0 && hit.transform.TryGetComponent(out interactable))
                return true;

            interactable = null;
            return false;
        }
    }
}