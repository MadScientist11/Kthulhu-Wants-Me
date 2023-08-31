namespace KthulhuWantsMe.Source.Gameplay.AbilitySystem
{
    public interface IAbilityResponse<TAbility> where TAbility : IAbility
    {
        void RespondTo(TAbility ability);
    }
}