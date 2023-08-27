namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public interface IPickable : IInteractable
    {
        bool Equipped { get; set; }

        void PickUp();
        void ThrowAway();
    }
}