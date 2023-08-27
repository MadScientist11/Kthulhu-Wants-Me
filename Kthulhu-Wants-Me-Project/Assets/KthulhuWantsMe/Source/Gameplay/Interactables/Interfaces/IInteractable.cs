using KthulhuWantsMe.Source.Gameplay.Interactables.Data;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public interface IInteractable
    {
        InteractableData InteractableData { get; }
        Transform Transform { get; }

        bool Interact();
    }
}