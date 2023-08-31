using KthulhuWantsMe.Source.Gameplay.Interactables.SOData;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces
{
    public interface IConsumable : IPickable
    {
        public ConsumableData ConsumableData { get; }
        int RemainingUses { get; set; }

        void Consume();
    }
}