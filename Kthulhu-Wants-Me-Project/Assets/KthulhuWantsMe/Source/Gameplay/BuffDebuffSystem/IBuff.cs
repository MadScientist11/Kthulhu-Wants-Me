using KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Interfaces.AutoInteractables
{

    public interface IBuff 
    {
        BuffTarget BuffTarget { get; }
        public BuffType BuffType { get; }
        public float Value { get; }
    }
}