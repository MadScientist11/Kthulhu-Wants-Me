using KthulhuWantsMe.Source.Gameplay.AbilitySystem;
using KthulhuWantsMe.Source.Gameplay.Interactables.SOData;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces
{
    public interface IConsumable : IPickable, IAbilityResponse<PlayerConsumeAbility>
    {
        public ConsumableData ConsumableData { get; }
        int RemainingUses { get; set; }

        void Consume();
    }
}