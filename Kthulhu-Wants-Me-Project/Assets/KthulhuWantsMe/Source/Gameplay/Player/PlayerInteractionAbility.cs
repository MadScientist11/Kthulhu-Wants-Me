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

    public class PlayerHighlightAbility : IAbility
    {
        public HighlightState HighlightState { get; private set; }
        private IInteractable _highlightedInteractable;

        private readonly RaycastHit[] _results = new RaycastHit[1];

        public void HighlightObjectOnMouseHover()
        {
            Ray worldRay = MousePointer.GetWorldRay(UnityEngine.Camera.main);
            if (HitInteractable(worldRay, out RaycastHit hit))
            {
                if (_highlightedInteractable != null && hit.transform == _highlightedInteractable.Transform)
                    return;

                if (hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    _highlightedInteractable = interactable;
                    HighlightState = HighlightState.Highlight;
                    interactable.RespondTo(this);
                }
            }
            else
            {
                HighlightState = HighlightState.CancelHighlight;
                _highlightedInteractable?.RespondTo(this);
                _highlightedInteractable = null;
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

    public class PlayerInteractionAbility : MonoBehaviour, IAbility
    {
        private PlayerHighlightAbility _playerHighlightAbility;

        private void Start()
        {
            _playerHighlightAbility = new PlayerHighlightAbility();
        }

     
        private void Update()
        {
            _playerHighlightAbility.HighlightObjectOnMouseHover();
        }

    
    }
}