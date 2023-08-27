using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public interface IInteractable
    {
        Transform Transform { get; }

        bool Interact();
    }
}