using System;
using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces;
using KthulhuWantsMe.Source.Gameplay.Interactables.SOData;
using KthulhuWantsMe.Source.Gameplay.Player;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public abstract class InteractableItem : MonoBehaviour, IInteractable
    {
        public Transform Transform => transform;
        public InteractableData InteractableData { get; }


        public abstract void RespondTo(PlayerInteractionAbility ability);

        public virtual void RespondTo(PlayerHighlightAbility ability)
        {
            switch (ability.HighlightState)
            {
                case HighlightState.Highlight:
                    GetComponent<Renderer>().material.SetFloat("_Outline_Width", 120);
                    GetComponent<Renderer>().material.SetFloat("_Offset_Z", -1);
                    break;
                case HighlightState.CancelHighlight:
                    GetComponent<Renderer>().material.SetFloat("_Outline_Width", 0);
                    GetComponent<Renderer>().material.SetFloat("_Offset_Z", 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}