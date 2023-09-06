using KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces.AutoInteractables;
using KthulhuWantsMe.Source.Gameplay.Player.PlayerAbilities;

namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public class Buff : IBuff
    {
        public BuffTarget BuffTarget { get; }
        public BuffType BuffType { get; }
        public float Value { get; }
        public void RespondTo(PlayerInteractionAbility ability)
        {
            
        }
    }
}