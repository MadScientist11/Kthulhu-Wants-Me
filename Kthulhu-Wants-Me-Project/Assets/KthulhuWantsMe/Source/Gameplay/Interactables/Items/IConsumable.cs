namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public interface IConsumable : IPickable
    {
        int MaxUses { get; }
        int RemainingUses { get; set; }

        void Consume();
    }
}