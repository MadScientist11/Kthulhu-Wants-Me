using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.SOData;
using KthulhuWantsMe.Source.Gameplay.Player;
using UnityEngine;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces
{
    public interface IInteractable : IAbilityResponse<PlayerInteractionAbility>
    {
        InteractableData InteractableData { get; }
        Transform Transform { get; }
    }
}