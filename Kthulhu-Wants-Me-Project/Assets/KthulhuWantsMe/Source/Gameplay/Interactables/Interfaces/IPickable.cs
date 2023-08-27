using KthulhuWantsMe.Source.Gameplay.Interactables.Data;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public interface IPickable : IInteractable
    {
        PickableData ItemData { get; }
        bool Equipped { get; set; }

    }
}