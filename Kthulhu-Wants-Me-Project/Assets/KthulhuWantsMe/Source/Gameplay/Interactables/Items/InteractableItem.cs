using System;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.SOData;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public abstract class InteractableItem : MonoBehaviour, IInteractable
    {
        public Transform Transform => transform;

        public InteractableData InteractableData { get; }

        [SerializeField, Range(0,200)] private float _outlineWidth;
        [SerializeField, Range(-10,10)] private float _offsetZ;
        
        private Material _interactableMaterial;
        
        private static readonly int OutlineWidth = Shader.PropertyToID("_Outline_Width");
        private static readonly int OffsetZ = Shader.PropertyToID("_Offset_Z");


        private void Awake() => 
            _interactableMaterial = GetComponent<Renderer>().material;

        public abstract void RespondTo(PlayerInteractionAbility ability);

        public virtual void RespondTo(PlayerHighlightAbility ability)
        {
            switch (ability.HighlightState)
            {
                case HighlightState.Highlight:
                    _interactableMaterial.SetFloat(OutlineWidth, _outlineWidth);
                    _interactableMaterial.SetFloat(OffsetZ, _offsetZ);
                    break;
                case HighlightState.CancelHighlight:
                    _interactableMaterial.SetFloat(OutlineWidth, 0);
                    _interactableMaterial.SetFloat(OffsetZ, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}