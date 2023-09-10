namespace KthulhuWantsMe.Source.Gameplay.BuffDebuffSystem
{
    public interface IUpdatableBuffDebuff : IBuffDebuff
    {
        void UpdateEffect();

        void CancelEffect();
    }
}