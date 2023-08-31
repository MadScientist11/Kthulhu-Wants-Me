using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.Data;
using KthulhuWantsMe.Source.Gameplay.Player;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public interface IPickable : IInteractable, IAbilityResponse<PlayerInventoryAbility>
    {
        PickableData ItemData { get; }
        bool Equipped { get; set; }


    }
}