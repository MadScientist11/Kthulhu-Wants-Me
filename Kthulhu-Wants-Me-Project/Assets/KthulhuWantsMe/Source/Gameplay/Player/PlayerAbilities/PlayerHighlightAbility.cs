using System;
using System.Linq;
using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Utilities;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Player
{
    public enum HighlightState
    {
        Highlight = 0,
        CancelHighlight = 1,
    }
    
    public class PlayerHighlightAbility : MonoBehaviour, IAbility
    {
        public HighlightState HighlightState { get; private set; }
        public IInteractable HighlightedInteractable { get; private set; }

        private readonly RaycastHit[] _results = new RaycastHit[1];


        private void Update() => 
            HighlightObjectOnMouseHover();

        private void HighlightObjectOnMouseHover()
        {
            Ray worldRay = MousePointer.GetWorldRay(UnityEngine.Camera.main);
            if (HitInteractable(worldRay, out RaycastHit hit))
            {
                if (HighlightedInteractable != null && hit.transform == HighlightedInteractable.Transform)
                    return;

                if (hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    HighlightedInteractable = interactable;
                    HighlightState = HighlightState.Highlight;
                    interactable.RespondTo(this);
                }
            }
            else
            {
                HighlightState = HighlightState.CancelHighlight;
                HighlightedInteractable?.RespondTo(this);
                HighlightedInteractable = null;
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