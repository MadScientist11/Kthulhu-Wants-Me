using KthulhuWantsMe.Source.Gameplay.Interactables.Data;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public interface IConsumable : IPickable
    {
        public ConsumableData ConsumableData { get; }
        int RemainingUses { get; set; }

        void Consume();
    }
}