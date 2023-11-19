using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.SOData;
using KthulhuWantsMe.Source.Gameplay.Player;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces
{
    public interface IPickable : IInteractable, IAbilityResponse<PlayerInventoryAbility>
    {
        PickableData ItemData { get; }
        bool Equipped { get; set; }
    }
}